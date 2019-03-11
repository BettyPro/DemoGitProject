using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Demo
{
    public enum UI
    {
        login = ManagerID.UIManager + 1,
        regist

    }

    public class LoadMsg : MsgBase
    {
        public string name;

        //public ushort id;
        public string des;

        public LoadMsg()
        {
        }

        public void ChangeInfo(string tmp_name, string tmp_des, ushort tmp_id)
        {
            this.name = tmp_name;
            this.des = tmp_des;
            //this.id = tmp_id;
            msgID = tmp_id; //最重要
        }

    }

    public class LoadMsgBack : MsgBase
    {
        public string name;
        public ushort id;
        public string des;
    }

    public class Load : UIBase
    {
        private LoadMsg instance;

        public LoadMsg Getinstance
        {
            get
            {
                if (instance == null)
                    instance = new LoadMsg();
                return instance;
            }
        }

        private void Awake()
        {
            msgIDs = new ushort[]
            {
                (ushort) UI.login,
                (ushort) UI.regist
            };
            RegistSelf(this, msgIDs);
        }

        // Use this for initialization
        void Start()
        {
            //msgIDs = new ushort[] { };
            //RegistSelf(this, msgIDs);
            //UIManager.Instance.RegistGameObject("login", gameObject);
            Debug.Log(gameObject);
            UIManager.Instance.GetGameObject("herobutton").GetComponent<UIBehaviour>().AddButtonListener(ButtonClick);
            //GameObject.Find("login").GetComponent<UIBehaviour>().AddButtonListener(ButtonClick);
            //SendMsg();
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void ButtonClick()
        {
            Debug.Log("点击了Login");
            for (int i = 0; i < msgIDs.Length; i++)
            {
                //Debug.Log(msgIDs.Length);
                //Debug.Log("msgid:--" + msgIDs[i]);
            }

            Getinstance.ChangeInfo("login", "这是一个login", 6005);
            //Getinstance.ChangeInfo("login", "这是一个login", msgIDs[1]);

            //LoadMsg load = new LoadMsg("login", "这是一个login", msgIDs[(int)UII.regist]);
            SendMsg(Getinstance);
            Debug.Log("发送了消息");
        }

        public override void ProcessEvent(MsgBase tmpMsg)
        {
            //base.ProcessEvent(tmpMsg);
            Debug.Log(22);
            LoadMsg ins = new LoadMsg();
            switch (tmpMsg.msgID)
            {
                case (ushort) UI.login:
                    Debug.Log("login收到了消息");
                    break;
                case (ushort) UI.regist:
                    Debug.Log("regist收到了消息");
                    break;
            }
        }
    }
}