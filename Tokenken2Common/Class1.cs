using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nancy;
using Nancy.Security;
using Nancy.Authentication.Forms;

namespace Tokenken2Common
{
    /// <summary>
    /// ユーザー情報
    /// </summary>
    public class UserInfo : IUserIdentity
    {
        private IEnumerable<string> _claims;

        public IEnumerable<string> Claims
        {
            get { return _claims; }
        }

        private string _userName;

        public string UserName
        {
            get { return _userName; }
        }

        public UserInfo(string userName)
            : this(userName, new string[] { }) { }

        public UserInfo(string userName, IEnumerable<string> claims)
        {
            this._userName = userName;
            this._claims = claims;
        }
    }

    public class UserDatabase : IUserMapper
    {
        //ユーザー情報ストア。サンプルの為、staticなメンバを使用している。
        private static List<Tuple<string, string, Guid>> users = new List<Tuple<string, string, Guid>>();

        public UserDatabase()
        {
            users.Add(new Tuple<string, string, Guid>("admin", "password", Guid.NewGuid()));
            users.Add(new Tuple<string, string, Guid>("user", "password", Guid.NewGuid()));
        }

        public IUserIdentity GetUserFromIdentifier(Guid identifier, NancyContext context)
        {
            var userRecord = users.Where(u => { return u.Item3 == identifier; }).FirstOrDefault();
            return userRecord == null
                ? null
                : new UserInfo(userRecord.Item1);
        }

        //IUserMapperインターフェイスには含まれていないが、ここでユーザー認証処理を定義している。
        //戻り値として、ログイン入力値に対応するユーザーID値を返す。
        public static Guid? ValidateUser(string username, string password)
        {
            var userRecord = users.Where(u => { return u.Item1 == username && u.Item2 == password; }).FirstOrDefault();

            if (userRecord == null)
            {
                return null;
            }

            return userRecord.Item3;
        }
    }
}
