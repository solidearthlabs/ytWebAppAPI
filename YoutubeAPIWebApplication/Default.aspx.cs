using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace YoutubeAPIWebApplication
{
    public partial class SampleForm : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            string videoID = txtVideoID.Text;
            YouTubeVideo video = new YouTubeVideo(videoID);
            LabelTitle.Text = video.title;
            LabelPublishDate.Text = video.publishedDate.ToShortDateString();

        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            string keyword = txtKeyword.Text;
            int monthsAgo = int.Parse(DropDownListMonthsAgo.SelectedItem.Value);
            
            //YoutubeVideoList videolist = new YoutubeVideoList(keyword);
            List<YoutubeVideo> videoDetailList = (List<YoutubeVideo>) Session["videoDetailList"];
            YouTubeAPI.KeywordSearch(keyword,monthsAgo,int.Parse(txtMaxResults.Text),int.Parse(txtMaxSubscribers.Text));
            Session["videoDetailList"] = YouTubeAPI.youtubeVideos;
            
            //lbKeywordSearchResults.DataSource = videolist.videos;
            //lbKeywordSearchResults.DataBind();
            GridViewKeywordSearchResults.DataSource = YouTubeAPI.youtubeVideos;
            GridViewKeywordSearchResults.DataBind();

            //for (int i = 0; i < GridViewKeywordSearchResults.HeaderRow.Cells.Count; i++)
            //{
            //    string titleCell = GridViewKeywordSearchResults.HeaderRow.Cells[i].Text;
            //    if (titleCell.Contains("keyword"))
            //    {
            //        GridViewKeywordSearchResults.Columns[i].Visible = false;
            //    }
            //}
                

            /*foreach (var item in videolist.videos)
            {
                lbKeywordSearchResults.Items.Add(item);

            }*/
        }
    }
}