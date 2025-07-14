//简单的UI层级显示管理(测试)
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using UnityEngine;

namespace ETHotfix
{

    public class Node {

        public GameObject parent;
        public GameObject current;
        public List<Node> childNodes;

        public Node() {

            childNodes = new List<Node>();
        }
    }


    public class UITrManager
    {
        
        static UITrManager mInstance;
        static public UITrManager Instance {
            get
            {
                if (mInstance == null) {

                    mInstance = new UITrManager();
                }
                return mInstance;
            }
        }

        Node root;
        Dictionary<GameObject, Node> dicNode;

        public UITrManager() {

            dicNode = new Dictionary<GameObject, Node>();
            root = new Node();
            root.parent = null;
            root.current = new GameObject();
            root.current.name = "UITrManager";
        }

        bool Append(Node n , GameObject p , GameObject o) {

            if (n.current.Equals(p)) {

                Node newNode = new Node();
                newNode.parent = p;
                newNode.current = o;
                n.childNodes.Add(newNode);//Log.Debug($"add node parent = {p.name} current = {o.name}");
                Avtive(newNode);
                return true;
            }
            else
            {
                for (int i = 0; i < n.childNodes.Count; ++i) {

                    if(Append(n.childNodes[i], p, o))
                    {
                        return true;
                    }
                }
                return false;
            }
        }

        bool Remove(Node n, GameObject o)
        {
            for (int i = 0; i < n.childNodes.Count; ++i) {

                if (n.childNodes[i].current.Equals(o)) {

                    Node rn = n.childNodes[i];
                    n.childNodes.Remove(rn);
                    //Log.Debug($"remove node parent = {rn.parent.name} current = {rn.current.name}");
                    UnActive(rn);
                    return true;
                }
                else
                {
                    if (Remove(n.childNodes[i] , o))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public void AppendAtRoot(GameObject o)
        {
            Append(root , root.current , o);
        }

        public void Append(GameObject p , GameObject o)
        {
            Append(root , p , o);
        }

        public void Remove(GameObject o) {

            Remove(root , o);
        }

        void Avtive(Node node) {

            node.parent.SetActive(false);
        }

        void UnActive(Node node) {

            node.parent.SetActive(true);
        }
    }
}
