using System;
using System.Collections.Generic;
using WinFormsApp.Configuration;
using WinFormsApp.Models;

namespace WinFormsApp.Services
{
    public class SessionService
    {
        private readonly Dictionary<string, object> _session = new Dictionary<string, object>();

        public void SetToken(string token)
        {
            _session["AUTH_TOKEN"] = token;
            /*Configuration.TOKKEN = token;*/
        }

        public string GetToken()
        {
            return "";
        }

        public void SetUser(User user)
        {
            _session["USER"] = user;
        }

        public User GetUser()
        {
            return _session.ContainsKey("USER") ? (User)_session["USER"] : null;
        }

        public void ClearSession()
        {
            _session.Clear();
            /*Configuration.TOKKEN = string.Empty;*/
        }
    }
}
