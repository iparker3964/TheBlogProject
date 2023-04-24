let index = 0;

function AddTag() {
    //get a reference to the tag element input
    var tagEntry = document.getElementById("tagEntry");

    //Lets use the search function to detect an error state
    let searchResult = Search(tagEntry.value);
    if (searchResult != null) {
        //trigger sweet alert
        sweetAlertWithDarkBtn.fire({
            html: `<span class='font-weight-border'>${searchResult.toUpperCase()}</span>`
        });
        //Swal.fire({
        //    title: 'Error!',
        //    text: 'Do you want to continue',
        //    icon: 'error',
        //    confirmButtonText: 'Cool'
        //});
    }
    else {
        //Create a new Select Option
        let newOption = new Option(tagEntry.value, tagEntry.value);
        document.getElementById("tagList").options[index++] = newOption;
    }

    //Clear out the tag entry control
    tagEntry.value = "";
    return true;
}

function DeleteTag() {
    var tagCount = 1;
    let tagList = document.getElementById("tagList");

    if (!tagList) return false;

    if (tagList.selectedIndex == -1) {
        sweetAlertWithDarkBtn.fire({
            html:"<span class='font-weight-bolder'>CHOOSE A TAG BEFORE DELETING</span>"
        });

        return true;
    }

    while (tagCount > 0) {
        let selectedIndex = tagList.selectedIndex;

        if (selectedIndex >= 0) {
            tagList.options[selectedIndex] = null;
            tagCount--;
        } else {
            tagCount = 0;
        }
        index--;
    }
    
}
/*
 * When I go to submit the form the data is not making it's way into the http post
 * To fix this issue I have to tell my code, the select list that when the form get's submitted.
 * Select all of the entries so that they make it into the post. By default the entries will not be selected.
 * So it will not be submitted to the post.
 */

$("form").on("submit", function () {
    $("#tagList option").prop("selected", "selected");
})

//Look for the tagValue variable to see if it has data
if (tagValues != '') {
    let tagArray = tagValues.split(",");

    for (let i = 0; i < tagArray.length;i++) {
        ReplaceTag(tagArray[i], i);

        index++;
    }
}

function ReplaceTag(tag,index) {
    let newOption = new Option(tag, tag);
    document.getElementById("tagList").options[index] = newOption;
}

function Search(str) {
    if (str == "") {
        return "Empty tags are not permitted.";
    }

    var tagElement = document.getElementById('tagList');

    if (tagElement) {
        let options = tagElement.options;

        for (let i = 0; i < options.length; i++) {
            if (options[i].value == str) {
                return `The tag #${str} is already in the list. Duplicates are not permitted.`
            }
        }
    }
}
const sweetAlertWithDarkBtn = Swal.mixin({
    customClass: {
        confirmButton: 'btn btn-danger rounded swal-wide btn-outline-dark'
    },
    imageUrl: '/images/oops.jpg',
    timer: 30000,
    buttonsStyling: false
});