using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System;
using DG.Tweening;

namespace ETHotfix
{
    public class UIJackPotLabelComponent
    {
        private ReferenceCollector rc;
        RectTransform transBase;
        HorizontalLayoutGroup hLayout;
        GameObject jpckpotItem;

        List<GameObject> itemList;
        List<int> displayNumberArray;
        float cellHeight;

        private int rowNumber = 9;
        private float normalModulus = 0.3f;
        private float bufferModulus = 0.7f;

        private int displayedNumber;
        bool isAnimation;
        List<Task> taskList;
        int finishedAnimationCount;

        private class Task
        {
            public int displayNumber;
            public int changeNumber;
            public bool animated;
        }

        private class Attribute
        {
            public float startDuration;
            public float startDelay;
            public float cycleDuration;
            public float endDuration;
            public int repeatCount;
            public int displayNumber;
        }

        enum ScrollAnimationDirection
        {
            Up,
            Down,
            Number
        }


        public void SetNumber(int number, bool animated)
        {
            number = StringHelper.GetRateGold(number);
            if (number == displayedNumber)
                return;
            if (isAnimation)
            {
                taskList.Add(new Task() { displayNumber = number, changeNumber = (number - displayedNumber), animated = animated });
            }
            else
            {
                isAnimation = true;
                if (animated)
                {
                    PlayAnimation(number - displayedNumber, number);
                }
                else
                {
                    List<int> displayNumbers = GetCellDisplayNumber(number);
                    for (int i = 0; i < displayNumbers.Count; i++)
                    {
                        SetScrollCellToNumber(i, displayNumbers[i]);
                    }
                    isAnimation = false;
                }
            }
            displayedNumber = number;
        }

        public UIJackPotLabelComponent(GameObject go, float width)
        {
            displayedNumber = 0;
            isAnimation = false;
            itemList = new List<GameObject>();
            taskList = new List<Task>();
            finishedAnimationCount = 0;

            rc = go.GetComponent<ReferenceCollector>();
            transBase = go.transform as RectTransform;
            hLayout = go.GetComponent<HorizontalLayoutGroup>();
            jpckpotItem = rc.Get<GameObject>("japot_item");

            float height = width * 135 / 768;
            transBase.sizeDelta = new Vector2(width, height);

            int padding = (int)(20 * width / 768);
            float spacing = 2 * width / 768;
            hLayout.padding = new RectOffset(padding, padding, (int)(padding * 0.7), (int)(padding * 1.2));
            hLayout.spacing = spacing;

            float cellWidth = (width - padding * 2 - (rowNumber - 1) * spacing) / rowNumber;
            cellHeight = cellWidth * 108 / 78;

            Transform tranImageNums = jpckpotItem.transform.Find("Image_nums");
            foreach (Transform numTran in tranImageNums)
            {
                (numTran.gameObject.transform as RectTransform).sizeDelta = new Vector2(cellWidth, cellHeight);
            }

            for (int i = 0; i < rowNumber; i++)
            {
                itemList.Insert(0, GetJackPotItem(go.transform));
            }
        }

        private void PlayAnimation(int changeNumber, int displayNumber)
        {
            List<int> repeatCountArray = GetRepeatTimes(changeNumber, displayNumber);
            List<int> willDisplayNums = GetCellDisplayNumber(displayNumber);

            float interval = GetInterval(displayNumber - changeNumber, displayNumber);

            ScrollAnimationDirection direction = (changeNumber > 0) ? ScrollAnimationDirection.Up : ScrollAnimationDirection.Down;

            float delay = 0.0f;

            if (repeatCountArray.Count != 0)
            {
                for (int i = 0; i < repeatCountArray.Count; i++)
                {
                    int repeatCount = repeatCountArray[i];
                    int willDisplayNum = willDisplayNums[i];
                    float startDuration = 0;

                    if (repeatCount == 0)
                    {
                        MakeSingleAnimation(i, interval, delay, repeatCountArray.Count, willDisplayNum);
                    }
                    else
                    {
                        if (direction == ScrollAnimationDirection.Up)
                        {
                            startDuration = (int)(interval * (10 - GetDisplayNumberOfCell(i)) / Mathf.Ceil(Mathf.Abs(changeNumber / Mathf.Pow(10, i))));
                            float cycleDuration = interval * 10 / Mathf.Abs(changeNumber / Mathf.Pow(10, i));
                            if (repeatCount == 1)
                            {
                                cycleDuration = 0;
                            }
                            float endDuration = bufferModulus * Mathf.Pow(willDisplayNum, 0.3f) / (i + 1);
                            Attribute attribute = new Attribute()
                            {
                                startDuration = startDuration,
                                startDelay = delay,
                                cycleDuration = cycleDuration,
                                endDuration = endDuration,
                                repeatCount = repeatCount - 1,
                                displayNumber = willDisplayNum
                            };
                            MakeMultiAnimation(i, direction, repeatCountArray.Count, attribute);
                        }
                        else if(direction == ScrollAnimationDirection.Down)
                        {
                            startDuration = (int)(interval * (GetDisplayNumberOfCell(i) - 0) / Mathf.Ceil(Mathf.Abs(changeNumber / Mathf.Pow(10, i))));
                            float cycleDuration = interval * 10 / Mathf.Abs(changeNumber / Mathf.Pow(10, i));
                            if (repeatCount == 1)
                            {
                                cycleDuration = 0;
                            }
                            float endDuration = bufferModulus * Mathf.Pow(10 - willDisplayNum, 0.3f) / (i + 1);
                            Attribute attribute = new Attribute()
                            {
                                startDuration = startDuration,
                                startDelay = delay,
                                cycleDuration = cycleDuration,
                                endDuration = endDuration,
                                repeatCount = repeatCount - 1,
                                displayNumber = willDisplayNum
                            };
                            MakeMultiAnimation(i, direction, repeatCountArray.Count, attribute);
                        }
                    }
                    delay = delay + startDuration;
                }
            }
        }

        private GameObject GetJackPotItem(Transform parent)
        {
            GameObject obj = GameObject.Instantiate(jpckpotItem);
            obj.gameObject.SetActive(true);
            obj.transform.SetParent(parent);
            obj.transform.localScale = Vector3.one;
            obj.transform.localPosition = Vector3.zero;
            return obj;
        }

        private List<int> GetCellDisplayNumber(int displayNumber)
        {
            List<int> displayCellNumbers = new List<int>();
            for (int i = 0; i < rowNumber; i++)
            {
                int tmpNumber = displayNumber % 10;
                displayCellNumbers.Add(tmpNumber);
                displayNumber = displayNumber / 10;
            }

            return displayCellNumbers;
        }

        private int GetDisplayNumberOfCell(int row)
        {
            GameObject go = itemList[row];
            RectTransform tran = go.transform.Find("Image_nums") as RectTransform;
            float y = tran.position.y;
            float tmpNumber = y / cellHeight;
            int displayNumber = (int)Mathf.Round(tmpNumber);
            return displayNumber;
        }

        private void SetScrollCellToNumber(int row, int number)
        {
            GameObject go = itemList[row];
            RectTransform tran = go.transform.Find("Image_nums") as RectTransform;
            float y = number * cellHeight;
            tran.anchoredPosition = new Vector2(0, y);
        }

        private List<int> GetRepeatTimes(int change, int number)
        {
            List<int> repeatTimesArray = new List<int>();
            int originNumber = number - change;
            if (change > 0)
            {
                do
                {
                    number = (number / 10) * 10;
                    originNumber = (originNumber / 10) * 10;
                    int repeat = (number - originNumber) / 10;

                    repeatTimesArray.Add(repeat);
                    number = number / 10;
                    originNumber = originNumber / 10;
                } while ((number - originNumber) != 0);
            }
            else
            {
                do
                {
                    number = (number / 10) * 10;
                    originNumber = (originNumber / 10) * 10;
                    int repeat = (originNumber - number) / 10;
                    repeatTimesArray.Add(repeat);
                    number = number / 10;
                    originNumber = originNumber / 10;
                } while ((originNumber - number) != 0);
            }
            return repeatTimesArray;
        }

        private float GetInterval(int originalNumber, int displayNumber)
        {
            List<int> repeatTimesList = GetRepeatTimes(displayNumber - originalNumber, displayNumber);
            int count = repeatTimesList.Count;
            int tmp1 = (int)(displayNumber / Math.Pow(10, count - 1));
            int tmp2 = (int)(originalNumber / Math.Pow(10, count - 1));

            int maxChangeNum = Math.Abs(tmp1 % 10 - tmp2 % 10);

            return normalModulus * count * maxChangeNum;
        }

        private void MakeSingleAnimation(int row, float duration, float delay, int animationCount, int displayNumber)
        {
            GameObject go = itemList[row];
            RectTransform tran = go.transform.Find("Image_nums") as RectTransform;
            float y = displayNumber * cellHeight;
            tran.DOAnchorPosY(y, duration).SetDelay(delay).SetEase(Ease.OutCubic).OnComplete(() =>
            {
                CheckTaskList(animationCount);
            });
        }

        private void CheckTaskList(int animationCount)
        {
            finishedAnimationCount++;
            if (finishedAnimationCount == animationCount)
            {
                finishedAnimationCount = 0;
                if (taskList.Count != 0)
                {
                    Task task = taskList[0];
                    taskList.RemoveAt(0);
                    int displayNumber = task.displayNumber;
                    if (task.animated)
                    {
                        int changeNumber = task.changeNumber;
                        PlayAnimation(changeNumber, displayNumber);
                    }
                    else
                    {
                        List<int> displayNumbers = GetCellDisplayNumber(displayNumber);
                        for (int i = 0; i < displayNumbers.Count; i++)
                        {
                            SetScrollCellToNumber(i, displayNumbers[i]);
                        }
                        CheckTaskList(1);
                    }
                }
                else
                {
                    isAnimation = false;
                }
            }
        }


        private void MakeMultiAnimation(int row, UIJackPotLabelComponent.ScrollAnimationDirection direction, int animationCount, Attribute attribute)
        {
            GameObject go = itemList[row];
            RectTransform tran = go.transform.Find("Image_nums") as RectTransform;
            float y = (direction == ScrollAnimationDirection.Up ? 10 : 0) * cellHeight;
            tran.DOAnchorPosY(y, attribute.startDuration).SetDelay(attribute.startDelay).SetEase(Ease.InCubic).OnComplete(() =>
            {
                SetScrollCellToNumber(row, (direction == ScrollAnimationDirection.Up ? 0 : 10));
                if (Math.Abs(attribute.cycleDuration) < 0.0001)
                {
                    tran.DOAnchorPosY(attribute.displayNumber * cellHeight, attribute.endDuration).SetEase(Ease.OutCubic).OnComplete(() =>
                    {
                        CheckTaskList(animationCount);
                    });
                }
                else
                {
                    float y3 = (direction == ScrollAnimationDirection.Up ? 10 : 0) * cellHeight;
                    tran.DOAnchorPosY(y3, attribute.cycleDuration).SetEase(Ease.Linear).SetLoops(attribute.repeatCount).OnComplete(() =>
                    {
                        SetScrollCellToNumber(row, (direction == ScrollAnimationDirection.Up ? 0 : 10));
                        tran.DOAnchorPosY(attribute.displayNumber * cellHeight, attribute.endDuration).SetEase(Ease.OutCubic).OnComplete(() =>
                        {
                            CheckTaskList(animationCount);
                        });
                    });
                }
            });
        }
    }
}
