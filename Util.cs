using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Providers.Entities;
using System.Web.SessionState; // For HttpSessionState

namespace WordBank {
  public class NumTotal {
    public int num;
    public int total;
    public void Set( int num, int total) {
      (this.num, this.total) = (num, total);
    }
    public void Inc(int inc) {
      num += inc;
      total++;
    }
    public string toStr() {
      return $"You got {num} of {total}.";
    }
    static public string Inc(HttpSessionState sesState, string ses_id, int num) {
      NumTotal numTotal = (NumTotal) sesState[ses_id];
      numTotal.Inc( num);
      sesState[ses_id] = numTotal;
      return numTotal.toStr();
    }
  }
}