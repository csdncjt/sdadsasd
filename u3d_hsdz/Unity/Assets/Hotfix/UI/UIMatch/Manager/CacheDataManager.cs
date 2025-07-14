using ETHotfix;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DtoSNS = ETHotfix.WEB2_sns_batch_relations.DataElement;

public class CacheDataManager
{

    private static CacheDataManager instance;
    public static CacheDataManager mInstance { get { if (instance == null) instance = new CacheDataManager(); return instance; } }

    Dictionary<string, DtoSNS> mDicRandomIdSNS;
    Dictionary<int, DtoSNS> mDicIdSNS;

    CacheDataManager()
    {
        mDicRandomIdSNS = new Dictionary<string, DtoSNS>();
        mDicIdSNS = new Dictionary<int, DtoSNS>();
    }

    public void SetSNSBatchRelations(List<DtoSNS> pDtos)
    {
        mDicRandomIdSNS = new Dictionary<string, DtoSNS>();
        mDicIdSNS = new Dictionary<int, DtoSNS>();
        for (int i = 0; i < pDtos.Count; i++)
        {
            var tDto = pDtos[i];
            mDicRandomIdSNS[tDto.randomNum] = tDto;
            mDicIdSNS[tDto.userId] = tDto;
        }
    }

    public void AddModifySNSBatchDto(DtoSNS pDto)
    {
        if (pDto == null) return;
        if (mDicRandomIdSNS == null)
        {
            mDicRandomIdSNS = new Dictionary<string, DtoSNS>();
            mDicRandomIdSNS[pDto.randomNum] = pDto;
            mDicIdSNS = new Dictionary<int, DtoSNS>();
            mDicIdSNS[pDto.userId] = pDto;
            Log.Debug("备注增加成功");
        }
        else
        {
            mDicRandomIdSNS[pDto.randomNum] = pDto;
            mDicIdSNS[pDto.userId] = pDto;
        }
    }


    public DtoSNS GetSNSBatchRelation(string pRandomId)
    {
        DtoSNS tDto = null;
        if (mDicRandomIdSNS.TryGetValue(pRandomId, out tDto))
        {
            return tDto;
        }
        return tDto;
    }

    public string GetRemarkName(int userId, string nick)
    {
        DtoSNS tDto = null;
        if (mDicIdSNS.TryGetValue(userId, out tDto))
        {
            return string.IsNullOrEmpty(tDto.remarkName) ? nick : tDto.remarkName; ;
        }
        return nick;
    }

    public int GetRemarkNameColor(int pUserId)
    {
        DtoSNS tDto = null;
        if (mDicIdSNS.TryGetValue(pUserId, out tDto))
        {
            return tDto.remarkColor;
        }
        return -1;
    }




}
