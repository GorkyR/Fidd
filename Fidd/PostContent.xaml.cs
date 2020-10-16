﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Reflection.Metadata;
using System.Text;
using System.Web;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Fidd
{
    /// <summary>
    /// Interaction logic for PostContent.xaml
    /// </summary>
    public partial class PostContent : UserControl
    {
        const int max_width = 800;
        static string css_style = @$"
            div.post-title, div.post-body {{ max-width: {max_width}px; margin: 0 auto; }}
            body {{ font-family: Calibri, sans-serif; font-size: 20px; margin: 30px; }}
            pre, code {{ font-family: Fira Code, monospace; font-size: 15px; color: #333; background-color: #eee; }}
            a, a:visited {{ color: blue; text-decoration: none; }}
            div.post-title > a {{ color: black; }}
            div.post-title > a:hover {{ color: blue;  }}
            div.post-title > a > h1 {{ font-size: 38px; font-weight: 800; }}

            div.post-body h1 {{ font-size: 32px; }}
            div.post-body h2 {{ font-size: 28px; }}
            div.post-body h3 {{ font-size: 25px; }}

            img, figure {{ max-width: 100%; display: block; border-radius: 5px; }}
            p > img {{ display: inline-block; }}
        ";

        Feed.Post _post = null;
        public Feed.Post Post
        {
            get => _post;
            set
            {
                string content = "<html></html>";
                if (value != null)
                {
                    content = @$"
                        <!doctype html>
                        <html>
                            <head>
                                <meta charset=""UTF-8"">
                                <style>{css_style}</style>
                            </head>
                            <body>
                                <div class=""post-title""><a href=""{value.Link}""><h1>{value.Title}</h1></a></div>
                                <div class=""post-body"">{App.FeedManager.LoadPostContent(value)}</div>
                            </body>
                        </html>";
                }
                WebContent.NavigateToString(content);
                _post = value;
            }
        }

        public PostContent()
        {
            InitializeComponent();
        }

        private void Navigating(object sender, NavigatingCancelEventArgs e)
        {
            if (e.Uri != null)
            {
                e.Cancel = true;
                var url = e.Uri.ToString();
                Process.Start(
                    new ProcessStartInfo(
                        "cmd", 
                        $"/c start {url.Replace("&", "^&")}"
                    ) { CreateNoWindow = true }
                );
            }
        }
    }
}