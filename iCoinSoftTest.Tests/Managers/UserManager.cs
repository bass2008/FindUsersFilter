using iCoinSoftTest.Models;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;

namespace iCoinSoftTest.Managers
{
    public class UserManager
    {
        private readonly ConcurrentDictionary<int, User> _allUsers = new ConcurrentDictionary<int, User>();

        private readonly ConcurrentDictionary<string, List<User>> _indexUsers = new ConcurrentDictionary<string, List<User>>();

        public void Add(User user)
        {
            _allUsers[user.Id] = user;
            
            var keys1 = GetKeys(user.Login.ToLower());
            var keys2 = GetKeys(user.Email.ToLower());
            var keys3 = GetKeys(user.Phone.ToLower());
            var keys = keys1.Union(keys2).Union(keys3).ToList();

            foreach (var key in keys)
            {
                if (_indexUsers.ContainsKey(key) == false)
                    _indexUsers[key] = new List<User>();

                _indexUsers[key].Add(user);

                // TODO: Заменить на алгоритм для частично отсортированных данных. (orderBy использует qsort)
                _indexUsers[key] = _indexUsers[key].OrderBy(x => x.Id).ToList();
            }
        }

        public bool Update(int userId, User user)
        {
            if (_allUsers.ContainsKey(user.Id) == false)
                return false;

            _allUsers[user.Id] = user;
            return true;
        }

        public List<User> Find(string filter, int limit)
        {
            return _indexUsers[filter].Take(limit).ToList();
        }

        private List<string> GetKeys(string source)
        {
            var keys = new List<string>();
            for (var i = 0; i < source.Length; i++)
            {
                for (var j = i + 1; j <= source.Length; j++)
                {
                    var str = source.Substring(i,j-i);
                    keys.Add(str);
                }
            }

            keys = keys.Distinct().ToList();

            return keys;
        }
    }
}
