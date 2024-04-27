using ST.Controls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UnitTest
{
    public partial class Form4 : Form
    {
        public Form4()
        {
            InitializeComponent();
            Load += Form4_Load;
            Click += Form4_Click;

            userEditor1.AutoCompleteShown += UserEditor1_AutoCompleteShown;

            KeyPreview = true;
            KeyDown += Form4_KeyDown;
        }

        private void Form4_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control == true && e.KeyCode == Keys.S)
            {
                
            }
        }

        private void UserEditor1_AutoCompleteShown(object sender, UserEditorShowAutoCompleteEventArg e)
        {
            List<string> t = new List<string>()
            {
                "asdf",
                "sdfg",
                "qwer"
            };

            e.Data = t;
        }

        private void Form4_Load(object sender, EventArgs e)
        {
        }

        private void Form4_Click(object sender, EventArgs e)
        {
            //userEditor1.Visible = false;
            //userEditor1.Text = "aaaa aaaa aa";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string t = "CREATE OR REPLACE PACKAGE PKG_{$0} AS\r\n/************************************************************************\r\n 설  명: {$1}\r\n 작성자: 안희성\r\n 작성일: {$2}\r\n 수정일:\r\n/***********************************************************************/\r\n\r\n    PROCEDURE {$4} (\r\no/list:1\r\ns/        IN_{1}        IN {3:encode(DATE:VARCHAR2;:{3})}\r\ns/\r\nb/        , IN_{1}        IN {3:encode(DATE:VARCHAR2;:{3})}\r\nb/ \r\n        -- Paging\r\n        , IN_PAGING_NO        IN NUMBER\r\n        , IN_RECORD_CNT        IN NUMBER\r\n        , OUT_CURSOR        OUT SYS_REFCURSOR\r\n    );\r\n    \r\n    PROCEDURE {$6} (\r\n        IN_CRUD                IN VARCHAR2\r\no/list:3\r\nb/        , IN_{1}    IN {3}\r\nb/\r\no/list:2\r\nb/        , IN_{1}    IN {3}\r\nb/\r\n        , IN_USER_ID        IN VARCHAR2\r\n        , IN_IPADD            IN VARCHAR2\r\n        , IN_DEPT_CD        IN VARCHAR2\r\n        , OUT_ERRYN            OUT NOCOPY VARCHAR2\r\n        , OUT_ERRMSG        OUT NOCOPY VARCHAR2\r\n    );\r\n    \r\nEND PKG_{$0};";
            userEditor1.Text = t;
            if (testBool)
            {
                userEditor1.Width /= 2;
            }
            else
            {
                userEditor1.Width *= 2;
            }
            testBool = !testBool;
        }

        bool testBool = false;
    }
}