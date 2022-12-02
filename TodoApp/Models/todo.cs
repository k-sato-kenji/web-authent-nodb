using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace TodoApp.Models
{
    public class todo
    {
        // ディスプレイNAME アノテーションを設定する。
        // ヘッダー部が日本語になる。
        // これがないと、プロパティー名がヘッダーに表示される。

        public int id { get; set; }
        
        [DisplayName("概要")]
        public string summary { get; set; }

        [DisplayName("詳細")]
        public string detail { get; set; }

        [DisplayName("期限")]
        public DateTime limit { get; set; }

        [DisplayName("完了！")]
        public bool done { get; set; }

    }
}