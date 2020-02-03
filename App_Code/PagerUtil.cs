using System;
using System.Xml.Xsl;

/// <summary>
/// PagerUtil 的摘要描述
/// </summary>
public class PagerUtil
{
    int RecordCount;
    int PageCount;
    int CurrentPageIndex;
    int StartPos;
    int EndPos;
    int StartPage;
    int EndPage;
    int PageSize;
    int PagerCount;

    public PagerUtil(int totalRec, int pageSize, int pagerCount)
    {
        RecordCount = totalRec;
        PageCount = Convert.ToInt32(Math.Ceiling((double)RecordCount / (double)pageSize));
        PageSize = pageSize;
        PagerCount = pagerCount;
    }

    public void SetCurrentPage(int curPage)
    {
        CurrentPageIndex = curPage;
        if (curPage < 1)
            CurrentPageIndex = 1;
        if (curPage > RecordCount)
            CurrentPageIndex = RecordCount;

        StartPos = (CurrentPageIndex - 1) * PageSize + 1;
        EndPos = StartPos + PageSize - 1;

        StartPage = ((CurrentPageIndex - 1) / PagerCount) * PagerCount + 1;
        EndPage = StartPage + PagerCount - 1;
        if (EndPage > PageCount)
            EndPage = PageCount;
    }

    public void AddXsltArguments(XsltArgumentList xArgs)
    {
        xArgs.AddParam("pagerRecordCount", "", RecordCount.ToString());
        xArgs.AddParam("pagerPageCount", "", PageCount.ToString());
        xArgs.AddParam("pagerCurrentPageIndex", "", CurrentPageIndex.ToString());
        xArgs.AddParam("pagerStartPos", "", StartPos.ToString());
        xArgs.AddParam("pagerEndPos", "", EndPos.ToString());
        xArgs.AddParam("pagerStartPage", "", StartPage.ToString());
        xArgs.AddParam("pagerEndPage", "", EndPage.ToString());
        xArgs.AddParam("pagerCount", "", PagerCount.ToString());
    }

    public int GetStartPos()
    {
        return this.StartPos;
    }

    public int GetEndPos()
    {
        return this.EndPos;
    }
}
