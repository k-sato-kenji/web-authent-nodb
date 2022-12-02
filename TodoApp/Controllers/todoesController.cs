using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TodoApp.Models;

namespace TodoApp.Controllers
{
    [Authorize]
    // サインインの状態でないと下記のメソッドは実行されない
    // 認証なしは、ＬＯＧＩＮ画面に戻される。
    public class todoesController : Controller // 必ず Controller を継承する。
    {
        //
        // 【C#】C#でhttp通信を行う方法。〜 Post通信・Get通信 〜
        //
        // http通信とは簡単に言うと通信方法です。主にWebページを見る際に使用されている通信方法になります。
        // もう少し詳細に言うと
        // Webページとなるhtmlが保存されているWebサーバに自身のPCからアクセスする際に使用する通信規格になります
        // 
        // その通信方法にはざっくり分けると
        // Get（データをもらう）通信と
        // Post（データを送る）通信の二つがあります。
        //
        //                   ActionResultについて   
        // クラス名              ヘルパーメソッド  概要
        // ViewResult            View              アクションメソッドに対応したViewを出力
        // RedirectToRouteResult RedirectToAction  指定のアクションメソッドに処理を転送
        // ContentResult         Content           指定されたテキストを出力
        // FileContentResult     File              指定されたファイルを出力
        // JsonResult            Json              指定されたオブジェクトをJSON形式で出力
        // HttpNotFoundResult    HttpNotFound      ４０４ページを出力
        // HttpStatusCodeResult                    HTTPのステータスコードを返す
        // EmptyResult                             何も行わない

        private TodoesContext db = new TodoesContext();　// オブジェクトを介してデータベースの操作を行う。
        //
        //  Create Read Update Delete (CRUD = クラッド)
        //
        //  インデックスメソッド（アクションメソッド）
        //  クライアントから要求されたURLを元に呼び出されるコントローラー
        // GET: todoes
        public ActionResult Index()
        {
            // アクションリザルト(ActionResult)に対応したをVIEWリザルトを返します。
            // VIEWリザルトはVIEWを表示するためのクラス
            // /Views/Todoes/Index.cshtml

            return View(db.todos.ToList());
        }

        // ディティールメソッドの定義  
        // GET: todoes/Details/5
        public ActionResult Details(int? id) //ブル型（idが未設定の場合、nullが設定される）
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);　//BadRequest code 400 です。 
            }
            todo todo = db.todos.Find(id); // id に一致するデータを一つ取り出す。
            if (todo == null)
            {
                return HttpNotFound(); // 一致しない場合、４０４ページを出力
            }
            return View(todo); // 一致すればデータをVIEWに設定して返す。
        }
        // クリエート文
        // GET: todoes/Create
        public ActionResult Create()
        {
            return View();
        }
        // クライアントからの[creat]ボタンがクリックされるとボストされます。
        // POST: todoes/Create
        // 過多ポスティング攻撃を防止するには、バインド先とする特定のプロパティを有効にしてください。
        // 詳細については、https://go.microsoft.com/fwlink/?LinkId=317598 を参照してください。
        // クライアンのアクションで此処に来る
        [HttpPost]  // ブラウザからPOSTのリクエストがあった際に呼び出される ACTION メソッドを表す、アノテーションです。
        [ValidateAntiForgeryToken]
        // クライアントかあらPOSTされたデータの名前を見て引数のポコ(POCO = Plain Old CLR Object)
        // POCO 特別なクラスやインターフェースを継承していないクラス
        // データ構造を表現する目的で使用します。
        public ActionResult Create([Bind(Include = "id,summary,detail,limit,done")] todo todo) // todo　モデルに紐づける
        {
            if (ModelState.IsValid)  //入力処理が正常の場合、
            {
                // Addでデータを設定する。
                db.todos.Add(todo);
                // dbセットの内容をデータベースに反映する。
                db.SaveChanges();
                // 一覧画面に戻る
                return RedirectToAction("Index");
            }

            return View(todo); // 入力がＮＧの場合、Create.cshtmlに返す。
        }

        // 編集文
        // DETAILS と CREATE を組み合わせた内容です。
        // GET: todoes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            todo todo = db.todos.Find(id);
            if (todo == null)
            {
                return HttpNotFound();
            }
            return View(todo);
        }

        // POST: todoes/Edit/5
        // 過多ポスティング攻撃を防止するには、バインド先とする特定のプロパティを有効にしてください。
        // 詳細については、https://go.microsoft.com/fwlink/?LinkId=317598 を参照してください。
        // クライアンのアクションで此処に来る
        [HttpPost]
        //  バリティードアンチフォジェリートークンアノテーションを記載でPOSTされてきたトークンを自動に検証してくれる。
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,summary,detail,limit,done")] todo todo) // データのバインドがされる
        {
            if (ModelState.IsValid)
            {
                // STATE プロバティに Modifiedを設定することで該当する箇所を更新出来る。
                db.Entry(todo).State = EntityState.Modified;
                // dbセットの内容をデータベースに反映する。
                db.SaveChanges();

                return RedirectToAction("Index");
            }
            return View(todo);
        }
        // 編集メソッドに似ている。
        // データを貰う。
        // GET: todoes/Delete/5　
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            todo todo = db.todos.Find(id);
            if (todo == null)
            {
                return HttpNotFound();
            }
            return View(todo); // Viewに返す。
        }

        // POST: todoes/Delete/5
        // クライアンのアクションで此処に来る
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            todo todo = db.todos.Find(id);
            db.todos.Remove(todo);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose(); //ディスポーザーは終了処理　保持してるコンテキストを開放する。
            }
            base.Dispose(disposing);
        }
    }
}
