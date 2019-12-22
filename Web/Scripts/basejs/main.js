function addAccount() {
    var formData = new FormData();
    formData.append('file', $('#photo')[0].files[0]);
    $.ajax({
        type: 'post',
        url: '/Home/Upload',
        data: formData,
        success: function () {
            
        },
        processData: false,
        contentType: false
    });

    $('#addForm').submit();
}