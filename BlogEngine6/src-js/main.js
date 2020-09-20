
/**********

Editor Functions

***********/

function deleteComment_Confirm(btnClicked) {

    var delConfirm = "<h3> Are you sure you want to feature this comment?</h3>";

    bootbox.confirm(delConfirm, function (result) {

        if (result) {

            deleteComment_Delete(btnClicked);

        }
    });
};

function deleteComment_Delete(btnClicked) {

    var $form = $(btnClicked).parents('form');

    $.ajax({
        type: "POST",
        url: "/Editor/RemoveComment",
        data: $form.serialize(),
        dataType: 'json',
        success: function (json) {


            if (json.success) {
                $(".commentItem").filter('[data-id="' + json.BlogCommentID + '"]').remove();
            }

        }
    });
};

function featureBlogItem_Confirm(btnClicked) {

    var blogID = $(btnClicked).parents('form').find('input[name="id"]').val();

    $.ajax({
        type: "GET",
        url: "/Blogs/DetailsJSON",
        data: { id: blogID },
        dataType: 'json',
        success: function (json) {
            var featureConfirm = "<h3> Are you sure you want to feature this blog post?</h3>" +
            "<h4>Title:</h4>" +
            "<p>" + json.Title + "</p>" +
            "<br/><h4>Post Date:</h4>" +
            "<p>" + json.PostDate + "</p>";
            bootbox.confirm(featureConfirm, function (result) {

                if (result) {

                    featureBlogItem_Feature(btnClicked);

                }

            });
        }
    });
};

function featureBlogItem_Feature(btnClicked) {

    var $form = $(btnClicked).parents('form');

    $.ajax({
        type: "POST",
        url: "/Editor/AddFeaturedArticle",
        data: $form.serialize(),
        dataType: 'json',
        success: function (json) {


            bootbox.alert("<h3>" + (json.success ? "Success" : "Error") + "</h3>" + "<p>" + json.msg + "</p>");

        }
    });
};


function removeFeaturedBlog_Confirm(btnClicked) {

    var blogID = $(btnClicked).parents('form').find('input[name="id"]').val();

    $.ajax({
        type: "GET",
        url: "/Blogs/DetailsJSON",
        data: { id: blogID },
        dataType: 'json',
        success: function (json) {
            var featureConfirm = "<h3> Are you sure you want to remove this featured blog post?</h3>" +
            "<h4>Title:</h4>" +
            "<p>" + json.Title + "</p>" +
            "<br/><h4>Post Date:</h4>" +
            "<p>" + json.PostDate + "</p>";
            bootbox.confirm(featureConfirm, function (result) {

                if (result) {

                    removeFeaturedBlog_Remove(btnClicked);

                }

            });
        }
    });
};


function removeFeaturedBlog_Remove(btnClicked) {

    var $form = $(btnClicked).parents('form');

    $.ajax({
        type: "POST",
        url: "/Editor/RemoveFeaturedArticle",
        data: $form.serialize(),
        dataType: 'json',
        success: function (json) {

            if (json.success) {
                $(".blogItem").filter('[data-id="' + json.blogId + '"]').remove();
            }

        }
    });
};




/**********

MyBlog Functions

***********/

function deleteBlogItem_Confirm(btnClicked, doRefresh) {

    var blogID = $(btnClicked).parents('form').find('input[name="id"]').val();

    $.ajax({
        type: "GET",
        url: "/Blogs/DetailsJSON",
        data: { id: blogID },
        dataType: 'json',
        success: function (json) {
            var deleteConfirm = "<h3> Are you sure you want to delete this blog post?</h3>" +
            "<h4>Title:</h4>"+
            "<p>" + json.Title + "</p>"+
            "<br/><h4>Post Date:</h4>" +
            "<p>" + json.PostDate + "</p>";
            bootbox.confirm(deleteConfirm, function (result) {

                if (result) {

                    deleteBlogItem_Delete(btnClicked, doRefresh);

                }

            });
        }
    });

    

};

function deleteBlogItem_Delete(btnClicked, doRefresh) {

    var $form = $(btnClicked).parents('form');

    $.ajax({
        type: "POST",
        url: "/MyBlog/Delete",
        data: $form.serialize(),
        dataType: 'json',
        success: function (json) {

            if (doRefresh) {
                window.location.replace("/MyBlog/Index");
            }
            else {
                $(".blogItem").filter('[data-id="' + json.blogId + '"]').remove();
            }
        }
    });

};



/**********

Blog Functions

***********/


function favoriteBlogItem(btnClicked) {
    var $form = $(btnClicked).parents('form');

    $.ajax({
        type: "POST",
        url: "/Blogs/FavoritePost",
        data: $form.serialize(),
        dataType: 'json',
        success: function (json) {

            // On success refresh comment partial view
            if (json.success) {
                
                if (json.isAdded) {
                    $(btnClicked).addClass("btn-danger");
                }
                else {
                    $(btnClicked).removeClass("btn-danger");
                }

            }
            else {
                bootbox.alert("<p><b>" + json.msg + "</b></p>");
            }
        }
    });
}



function postBlogComment(btnClicked, count) {
    var $form = $(btnClicked).parents('form');

    $.ajax({
        type: "POST",
        url: "/Blogs/PostComment",
        data: $form.serialize(),
        dataType: 'json',
        success: function (json) {

            // On success refresh comment partial view
            if (json.success) {
                $form.find("textarea").val("");
                $('.blogCommentArea').load('/Blogs/BlogComments?id=' + json.blogId + '&count=' + count);
            }
            else {
                bootbox.alert("<p><b>"+json.msg+"</b></p>");
            }
        }
    });
}



$(document).ready(function () {

    // If user is not logged in show login modal on authorized areas
    if (typeof IS_AUTH === 'undefined') {
        $(".navbar-nav li a.requireLogin").click(function (e) {
            e.preventDefault();

            $("#ModalLogin").modal("show");

            return false;
        });
    }


});
