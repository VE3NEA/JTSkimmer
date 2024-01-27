using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace JTSkimmer
{  internal class RecentCalls : ConcurrentDictionary<string, DateTime>
  {
    public void DeleteExpired(TimeSpan age)
    {
      DateTime minTime = DateTime.UtcNow - age;
      var keys = this.Where(c => c.Value < minTime).Select(c => c.Key);
      foreach (var key in keys) TryRemove(key, out DateTime _);
    }

    public bool AddIfNew(string key)
    {
      if (ContainsKey(key)) return false;
      this[key] = DateTime.UtcNow;
      return true;
    }
  }
}
