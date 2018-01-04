
$(function () {
});


function signOut(phone) {
    $.ajax({
        type: "POST",
        dataType: "json",
        url: '/Account/SignOutRequest',
        data: { phone: phone },
        success: function (result) {
            if (!result.Status) {
                if (result.StatusCode.indexOf('SO10') != -1) {

                }
            }
            else {
                window.location = '/Account/Login';
            }
        },
        error: function (error) {
            alert("Ajax send error");
        }
    });
}