using System;
using System.Collections.Generic;

namespace NineBizlogistics.Config
{
    public class JsonModel:JsonModelBase
    {
        Dictionary<string, object> dic;

        public void SetError(string error)
        {
            Data = null;
            CanContinue  = false;
            Status = 0;
            Error = error;
        }
        public void SetOK()
        {
            Data = null;
            Status = 1;
            Error = "";
        }
        public void SetContent(object o)
        {
            Data = o;
            Status = 1;
            Error = "";
        }
        public JsonModel Add(string key, object value)
        {
            if (CanContinue )
            {
                if (dic == null)
                {
                    dic = new Dictionary<string, object>();
                }
                if (dic.ContainsKey(key)) { dic[key] = value; }
                else
                {
                    dic.Add(key, value);
                }
                Data = dic;
            }
            return this;
        }

        public bool Check(string Err, Func<bool> f)
        {
            if (CanContinue )
            {
                CanContinue = f.Invoke();
                if (!CanContinue)
                {
                    SetError(Err);
                }
            }
            return CanContinue;
        }
    }
}
