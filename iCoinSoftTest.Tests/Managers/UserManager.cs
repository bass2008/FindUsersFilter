using iCoinSoftTest.Models;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;

namespace iCoinSoftTest.Managers
{
    public class UserManager
    {
        private readonly ConcurrentDictionary<int, User> _allUsers = new ConcurrentDictionary<int, User>();

        private readonly ConcurrentDictionary<string, SortedList<int, User>> _indexUsers = new ConcurrentDictionary<string, SortedList<int, User>>();

        public void Add(User user)
        {
            _allUsers[user.Id] = user;

            var set = new HashSet<string>();

            AddKeys(set, user.Login.ToLower());
            AddKeys(set, user.Email.ToLower());
            AddKeys(set, user.Phone.ToLower());

            foreach (var key in set)
            {
                if (_indexUsers.ContainsKey(key) == false)
                    _indexUsers[key] = new SortedList<int, User>();

                _indexUsers[key].Add(user.Id, user);
            }
        }

        public bool Update(int userId, User user)
        {
            if (_allUsers.ContainsKey(userId) == false)
                return false;

            _allUsers[userId] = user;
            return true;
        }

        public List<User> Find(string filter, int limit)
        {
            if (_indexUsers.ContainsKey(filter) == false)
                return new List<User>();

            return _indexUsers[filter].Values.Take(limit).ToList();
        }

        private void AddKeys(HashSet<string> set, string source)
        {
            for (var i = 0; i < source.Length; i++)
            {
                for (var j = i + 1; j <= source.Length; j++)
                {
                    var str = source.Substring(i,j-i);
                    set.Add(str);
                }
            }
        }
    }
}
