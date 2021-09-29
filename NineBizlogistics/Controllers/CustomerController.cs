using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using NineBizlogistics.Config;
using NineBizlogistics.Model;

namespace NineBizlogistics.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="uid">uid</param>
        /// <returns></returns>
        [HttpPost]
        public JsonModel Delete(string uid)
        {
            JsonModel J = new JsonModel();
            J.Check("未发现该代理", () => Customer.Exist(zz => zz.Uid == uid));
            if (J.CanContinue)
            {
                Customer.Delete(zz => zz.Uid == uid);
            }
            return J;
        }

        [HttpPost]
        public JsonModel AddOrUpdate(CustomerReq customerReq)
        {
            JsonModel J = new JsonModel();
            // 添加
            if (customerReq.IsAdd)
            {
                Customer newCustomer = new Customer()
                {
                    ContractId = customerReq.ContractId,
                    Name = customerReq.Name,
                    Phone = customerReq.Phone,
                    Product = customerReq.Product,
                    Area = customerReq.Area,
                    QQ = customerReq.QQ,
                    HigherLevelWechat = customerReq.HigherLevelWechat,
                    Level = customerReq.Level,
                    StartTime = customerReq.StartTime,
                    EndTime = customerReq.EndTime,
                    Note = customerReq.Note
                };
                Customer.Add(newCustomer);
            }
            else
            {
                Customer oldCus = Customer.FirstOrDefault(zz => zz.Uid == customerReq.Uid);
                J.Check("该代理不存在", () => oldCus != null);
                if (J.CanContinue)
                {
                    oldCus.ContractId = customerReq.ContractId;
                    oldCus.Name = customerReq.Name;
                    oldCus.Phone = customerReq.Phone;
                    oldCus.Product = customerReq.Product;
                    oldCus.Area = customerReq.Area;
                    oldCus.QQ = customerReq.QQ;
                    oldCus.HigherLevelWechat = customerReq.HigherLevelWechat;
                    oldCus.Level = customerReq.Level;
                    oldCus.StartTime = customerReq.StartTime;
                    oldCus.EndTime = customerReq.EndTime;
                    oldCus.Note = customerReq.Note;
                    Customer.UpdateById(oldCus);
                }
            }
            return J;
        }

        [HttpPost]
        public JsonModel List(string contractId, string phone, string wechatId, string name, string higherLevelWechat, int Page = 1, int PageSize = 10)
        {
            JsonModel J = new JsonModel();
            J.Check("参数错误", () => Page >= 1);
            J.Check("参数错误", () => PageSize >= 1);
            if (J.CanContinue)
            {
                var f = CreatExpression(contractId, phone, wechatId, name, higherLevelWechat);
                J.SetContent(Customer.TakePageDesc(f, Page, PageSize));
            }
            return J;
        }

        [HttpPost]
        public JsonModel Count(string contractId, string phone, string wechatId, string name, string higherLevelWechat)
        {
            var f = CreatExpression(contractId, phone, wechatId, name, higherLevelWechat);
            JsonModel J = new JsonModel();
            J.SetContent(Customer.Count(f));
            return J;
        }

        ExpressionFilter<Customer> CreatExpression(string contractId, string phone, string wechatId, string name, string higherLevelWechat)
        {
            ExpressionFilter<Customer> f = new ExpressionFilter<Customer>();
            if (!string.IsNullOrWhiteSpace(contractId))
            {
                f.And(zz => zz.ContractId.Contains(contractId));
            }
            if (!string.IsNullOrWhiteSpace(phone))
            {
                f.And(zz => zz.Phone.Contains(phone));
            }
            if (!string.IsNullOrWhiteSpace(wechatId))
            {
                f.And(zz => zz.Phone.Contains(wechatId));
            }
            if (!string.IsNullOrWhiteSpace(name))
            {
                f.And(zz => zz.Phone.Contains(name));
            }
            if (!string.IsNullOrWhiteSpace(higherLevelWechat))
            {
                f.And(zz => zz.Phone.Contains(higherLevelWechat));
            }
            return f;
        }
    }
}
