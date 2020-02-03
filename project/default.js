/*pageList*/

/*===============*/
/*init*/

jQuery(document).ready(function ()
{
});


/*===============*/
/*===搜尋*/
function doSearch()
{
    location.href = "default.aspx?q=" + encodeURIComponent($('#keyword').val());
}


