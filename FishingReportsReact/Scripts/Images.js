
function uploadNewFiles(ajaxUrl, ajaxDeleteUrl) {
    // Checking whether FormData is available in browser
    if (window.FormData !== undefined) {
        var fileUpload = $("#NewFileUpload").get(0);
        var files = fileUpload.files;

        // Create FormData object
        var fileData = new FormData();

        // Looping over all files and add it to FormData object
        for (var i = 0; i < files.length; i++) {
            fileData.append(files[i].name, files[i]);
        }

        $.ajax({
            url: ajaxUrl,
            type: "POST",
            contentType: false, // Not to set any content header
            processData: false, // Not to process data
            data: fileData,
            success: function (result) {
                if (result.success) {
                    for (var index = 0; index < result.files.length; index++) {
                        createUploadedImage(result.files[index].Value, result.files[index].Text, ajaxDeleteUrl);
                    }
                }
                else {
                    alert(result.responseText);
                }
            },
            error: function (xhr, ajaxOptions, thrownError) {
                alert(xhr.status);
            }
        });
    }
    else {
        alert("FormData is not supported.");
    }
}

function createUploadedImage(imageId, imagePath, deleteAjaxUrl) {
    var imageParagraph = document.createElement('p');
    imageParagraph.id = "image" + imageId;

    var deleteButton = document.createElement("input");
    deleteButton.onclick = (function (imageId) {
        return function () {
            deleteImage(imageId, deleteAjaxUrl);
        }
    })(imageId);
    deleteButton.value = "Remove";
    deleteButton.type = "button";
    deleteButton.style.marginLeft = "5px";

    var image = document.createElement('img');
    image.src = imagePath;
    image.alt = "New Image";
    image.title = "New Image";

    imageParagraph.appendChild(image);
    imageParagraph.appendChild(deleteButton);
    document.getElementById("uploadedFilesSection").appendChild(imageParagraph);
    return image;
}

function deleteImage(imageId, ajaxUrl) {
    $.ajax({
        url: ajaxUrl,
        type: "POST",
        contentType: 'application/json', // Not to set any content header
        data: JSON.stringify({ imageId: imageId }),
        success: function (result) {
            var elem = document.getElementById('image' + imageId);
            elem.parentNode.removeChild(elem);
        },
        error: function (xhr, ajaxOptions, thrownError) {
            alert(xhr.status);
        }
    });
}

function deleteExistingImage(elem, ajaxUrl) {
    var id = $(elem).data('assigned-id');

    deleteImage(id, ajaxUrl);
}

