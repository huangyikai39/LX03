using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Service.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : Controller
    {
        [HttpGet]
        public ActionResult Get()
        {
            return Json(DAL.UserInfo.Instance.GetCount());
        }
        [HttpPut("{username}")]
        public ActionResult getUser(string username)
        {
            var result = DAL.UserInfo.Instance.GetModel(username);
            if (result != null)
                return Json(Result.Ok(result));
            else
                return Json(Result.Err("�û���������"));
        }
        [HttpPost]
        public ActionResult Post([FromBody] Model.UserInfo users)
        {
            try
            {
                int n = DAL.UserInfo.Instance.Add(users);
                return Json(Result.Ok("��ӳɹ�"));
            }
            catch (Exception ex)
            {
                if (ex.Message.ToLower().Contains("primary"))
                    return Json(Result.Err("�û����Ѵ���"));
                else if (ex.Message.ToLower().Contains("null"))
                    return Json(Result.Err("�û��������롢��ݲ���Ϊ��"));
                else
                    return Json(Result.Err(ex.Message));
            }
        }
        [HttpPut]
        public ActionResult Put([FromBody] Model.UserInfo users)
        {
            try
            {
                var n = DAL.UserInfo.Instance.Update(users);
                if (n != 0)
                    return Json(Result.Ok("�޸ĳɹ�"));
                else
                    return Json(Result.Err("�û���������"));
            }
            catch (Exception ex)
            {
                if (ex.Message.ToLower().Contains("null"))
                    return Json(Result.Err("���롢��ݲ���Ϊ��"));
                else
                    return Json(Result.Err(ex.Message));
            }
        }
        [HttpDelete("{username}")]
        public ActionResult Delete(string username)
        {
            try
            {
                var n = DAL.UserInfo.Instance.Delete(username);
                if (n != 0)
                    return Json(Result.Ok("ɾ���ɹ�"));
                else
                    return Json(Result.Err("�û�������"));
            }
            catch (Exception ex)
            {
                if (ex.Message.ToLower().Contains("foreign"))
                    return Json(Result.Err("��������Ʒ�����û�����ɾ��"));
                else
                    return Json(Result.Err(ex.Message));
            }
        }
        [HttpPost("check")]
        public ActionResult UserCheck([FromBody] Model.UserInfo users)
        {
            try
            {
                var result = DAL.UserInfo.Instance.GetModel(users.userName);
                if (result == null)
                    return Json(Result.Err("�û�������"));
                else if (result.passWord == users.passWord)
                {
                    if (result.type == "����Ա")
                    {
                        result.passWord = "********";
                        return Json(Result.Ok("����Ա��¼�ɹ�", result));
                    }
                    else
                        return Json(Result.Err("ֻ�й���Ա�ܹ������̨����"));
                }
                else
                    return Json(Result.Err("�������"));
            }
            catch (Exception ex)
            {
                return Json(Result.Err(ex.Message));
            }
        }
        [HttpPost("genCheck")]
        public ActionResult genUserCheck([FromBody] Model.UserInfo users)
        {
            try
            {
                var result = DAL.UserInfo.Instance.GetModel(users.userName);
                if (result == null)
                    return Json(Result.Err("�û�������"));
                else if (result.passWord == users.passWord)
                {
                    result.passWord = "********";
                    return Json(Result.Ok("��¼�ɹ�", result));
                }
                else
                    return Json(Result.Err("�������"));
            }
            catch (Exception ex)
            {
                return Json(Result.Err(ex.Message));
            }
        }
        [HttpPost("page")]
        public ActionResult getPage([FromBody] Model.Page page)
        {
            var result = DAL.UserInfo.Instance.GetPage(page);
            if (result.Count() == 0)
                return Json(Result.Err("���ؼ�¼��Ϊ0"));
            else
                return Json(Result.Ok(result));
        }
    }
}