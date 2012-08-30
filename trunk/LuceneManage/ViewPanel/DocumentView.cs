using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using LuceneManage.IndexWapper;
using LuceneManage.LuceneAPI;
using Lucene.Net.Index;
using Lucene.Net.Documents;
using System.Collections;
using LuceneManage.Helpers;
using Lucene.Net.Analysis;
using Lucene.Net.Analysis.Standard;
using Lucene.Net.Store;
using System.IO;
using StockFoo.Analyzer;
using Lucene.Net.Search;

namespace LuceneManage.ViewPanel {
    public partial class DocumentView : UserControl, IUpdateUI {
        public DocumentView() {
            InitializeComponent();
        }

        private int docNum;

        public void UpdateUI() {
            if (CurrentIndex.IsIndexBeOpend()) {
                IndexOpen open = CurrentIndex.GetCurrentOpendIndex();
                docNum = open.Reader.MaxDoc();
                label1.Text = string.Format("共有文档{0}条", docNum);

                FieldTermsRelation rela = FieldTermsRelation.getInstance();
                for (int i = 0; i < rela.Fields.Length; i++) {
                    comboBox1.Items.Add(rela.Fields[i]);
                }
                if (comboBox1.Items.Count > 0)
                    comboBox1.SelectedIndex = 0;
            }
        }

        private void button1_Click(object sender, EventArgs e) {
            int id = getDocumentID();
            if (id > 0)
                id--;
            textBox1.Text = id.ToString();
            FindDocuemnt(id);
            label9.Text = "当前为文档" + (id + 1).ToString();
        }

        private int getDocumentID() {
            int id = 0;
            int.TryParse(textBox1.Text, out id);
            return id;
        }

        private void button2_Click(object sender, EventArgs e) {
            int id = getDocumentID();
            if (id < docNum - 1)
                id++;
            textBox1.Text = id.ToString();
            FindDocuemnt(id);
            label9.Text = "当前为文档" + (id + 1).ToString();
        }

        private void button3_Click(object sender, EventArgs e) {
            int id = getDocumentID();
            FindDocuemnt(id);
            label9.Text = "当前为文档" + (id + 1).ToString();
        }

        private void FindDocuemnt(int id) {
            ShowDocument(id);
        }

        private void DocumentView_Load(object sender, EventArgs e) {
            int id = getDocumentID();
            FindDocuemnt(id);
            label9.Text = "当前为文档" + (id + 1).ToString();
        }

        private void button4_Click(object sender, EventArgs e) {
            BindFieldTermFirst();
        }

        private void button5_Click(object sender, EventArgs e) {
            FieldTermsRelation rela = FieldTermsRelation.getInstance();
            TermModel model = rela.FindTerm(comboBox1.Text, textBox2.Text, false);
            if (model != null) {
                textBox2.Text = model.Term.Text();

                label3.Text = string.Format("当前词频：{0}", model.Count);

                BindTermDocs(model.Term);
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e) {
            BindFieldTermFirst();
        }

        private int termDocPtr = 0;
        private IList<TermDocumentsRelation.TermDoc> docs;

        private void BindTermDocs(Term term) {
            if (CurrentIndex.IsIndexBeOpend()) {
                IOpen open = CurrentIndex.GetCurrentOpendIndex();
                TermDocumentsRelation tdr = new TermDocumentsRelation(open);
                docs = tdr.DocumentCount(term);

                label6.Text = string.Format("文档数：{0}", docs.Count);
            }
        }

        private void BindFieldTermFirst() {
            FieldTermsRelation rela = FieldTermsRelation.getInstance();
            TermModel model = rela.FindTerm(comboBox1.Text, null, true);
            if (model != null) {
                textBox2.Text = model.Term.Text();

                label3.Text = string.Format("当前词频：{0}", model.Count);

                BindTermDocs(model.Term);
            }
        }

        private void button6_Click(object sender, EventArgs e) {
            termDocPtr = 0;
            ShowCurrentDocument();
        }

        private void button7_Click(object sender, EventArgs e) {
            if (termDocPtr < docs.Count - 1) {
                termDocPtr++;
            } 
            ShowCurrentDocument();
        }

        private void ShowCurrentDocument() {
            if (docs.Count > 0) {
                TermDocumentsRelation.TermDoc doc = docs[termDocPtr];
                label8.Text = string.Format(label8.Text, doc.Freq);
                ShowDocument(doc.Doc);
            }
        }

        private void ShowDocument(int docId) {
            listView1.Clear();
            listView1.Columns.Add("Field", 120);
            listView1.Columns.Add("Norm", 120);
            listView1.Columns.Add("Text", 400);
            IOpen open = CurrentIndex.GetCurrentOpendIndex();
            if (docId > open.Reader.NumDocs())
            {
                return;
            }
            
            Document doc = open.Reader.Document(docId);
       
            IList list = doc.GetFields();
            txttitle.Text = ((Field)doc.GetFields()[0]).StringValue();
            labtitleboost.Text = "norm:" + TermDocumentsRelation.GetNorm(CurrentIndex.GetCurrentOpendIndex().Reader, ((Field)doc.GetFields()[0]).Name(), docId).ToString();
            txturl.Text = ((Field)doc.GetFields()[1]).StringValue();
            laburlboost.Text = "norm:" + TermDocumentsRelation.GetNorm(CurrentIndex.GetCurrentOpendIndex().Reader, ((Field)doc.GetFields()[1]).Name(), docId).ToString();
            txtsite.Text = ((Field)doc.GetFields()[2]).StringValue();
            labsiteboost.Text = "norm:" + TermDocumentsRelation.GetNorm(CurrentIndex.GetCurrentOpendIndex().Reader, ((Field)doc.GetFields()[2]).Name(), docId).ToString();
            txtbody.Text = ((Field)doc.GetFields()[3]).StringValue();
            labbodyboost.Text = "norm:" + TermDocumentsRelation.GetNorm(CurrentIndex.GetCurrentOpendIndex().Reader, ((Field)doc.GetFields()[3]).Name(), docId).ToString();
            txtpublishtime.Text = ((Field)doc.GetFields()[4]).StringValue();
            labpublishtimeboost.Text = "norm:" + TermDocumentsRelation.GetNorm(CurrentIndex.GetCurrentOpendIndex().Reader, ((Field)doc.GetFields()[4]).Name(), docId).ToString();
            labdocumentboost.Text = doc.GetBoost().ToString();
            
            for (int i = 0; i < list.Count; i++) {
                Field f = list[i] as Field;
                listView1.Items.Add(new ListViewItem(new string[] { f.Name(), TermDocumentsRelation.GetNorm(CurrentIndex.GetCurrentOpendIndex().Reader, ((Field)doc.GetFields()[i]).Name(), docId).ToString(), f.StringValue() }));
            }
            
        }

        private void btnUpdateDoc_Click(object sender, EventArgs e)
        {
            try
            {
                string path = System.Configuration.ConfigurationSettings.AppSettings["Data"].ToString();
                Analyzer analyzer = new StockFooAnalyzer(path);
                FSDirectory dy = FSDirectory.Open(new DirectoryInfo(System.Configuration.ConfigurationSettings.AppSettings["IndexDirectory"].ToString()));
                IndexWriter indexWriter = new IndexWriter(dy, analyzer, false);
                IOpen oldopen = CurrentIndex.GetCurrentOpendIndex();
                Document olddoc = oldopen.Reader.Document(Convert.ToInt32(textBox1.Text));
                string oldurl = olddoc.Get("url").ToString();
                bool oldisnew = false;
                if (oldurl.Trim().Equals(txturl.Text.Trim()))
                    oldisnew = true;
                Document document = new Document();
                Field ftitle = new Field("title", txttitle.Text, Field.Store.YES, Field.Index.ANALYZED);//存储，索引
                document.Add(ftitle);
                Field furl = new Field("url", txturl.Text, Field.Store.YES, Field.Index.NOT_ANALYZED);//存储，不索引
                document.Add(furl);
                Field fsite = new Field("site", txtsite.Text, Field.Store.YES, Field.Index.NOT_ANALYZED);//存储，不索引
                document.Add(fsite);
                Field fbody = new Field("body", txtbody.Text, Field.Store.YES, Field.Index.ANALYZED);//存储，索引
                document.Add(fbody);
                Field fpublishtime = new Field("publish_time", txtpublishtime.Text, Field.Store.YES, Field.Index.NOT_ANALYZED);//存储，不索引
                document.Add(fpublishtime);

                Term term = new Term("url", txturl.Text);
                indexWriter.UpdateDocument(term, document, analyzer);

                indexWriter.Optimize();
                indexWriter.Close();
               
                IndexOpen open = new IndexOpen(System.Configuration.ConfigurationSettings.AppSettings["IndexDirectory"].ToString(), false);
                 bool isOpend = open.Open();
                 if (isOpend)
                 {
                     CurrentIndex.SetCurrentOpendIndex(open);
                 }
                 if (!oldisnew)
                 {
                     FindDocuemnt(0);
                     label9.Text = "当前为文档1";
                     textBox1.Text = "0";
                     docNum = docNum + 1;
                     label1.Text = string.Format("共有文档{0}条", docNum);
                 }
                 else
                 {
                     docNum = open.Reader.MaxDoc();
                     FindDocuemnt(docNum - 1);
                     label9.Text = "当前为文档" + (docNum).ToString();
                     textBox1.Text = (docNum - 1).ToString();
                 }
                 MessageBox.Show("更新成功！");
            }
            catch (Exception ex)
            {
                MessageBox.Show("更新失败！\\n"+ex.Message);
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
             try
            {
                if (MessageBox.Show("确认删除该文档记录吗？", "提示：", MessageBoxButtons.OKCancel) == DialogResult.OK)
                {
                    string path = System.Configuration.ConfigurationSettings.AppSettings["Data"].ToString();
                    Analyzer analyzer = new StockFooAnalyzer(path);
                    FSDirectory dy = FSDirectory.Open(new DirectoryInfo(System.Configuration.ConfigurationSettings.AppSettings["IndexDirectory"].ToString()));
                    IndexWriter indexWriter = new IndexWriter(dy, analyzer, false);
                    IOpen open = CurrentIndex.GetCurrentOpendIndex();
                    Document olddoc = open.Reader.Document(Convert.ToInt32(textBox1.Text));
                    indexWriter.DeleteDocuments(new Term("url", olddoc.Get("url")));

                    indexWriter.Optimize();
                    indexWriter.Close();

                    FindDocuemnt(0);
                    label9.Text = "当前为文档1";
                    textBox1.Text = "0";
                    docNum = docNum - 1;
                    label1.Text = string.Format("共有文档{0}条", docNum);

                    MessageBox.Show("删除成功！");
                }
            }
             catch (Exception ex)
             {
                 MessageBox.Show("删除失败！\\n" + ex.Message);
             }
        }

        private void listView1_AfterLabelEdit(object sender, LabelEditEventArgs e)
        {
            
        }
    }
}
