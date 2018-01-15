
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
                alert(result.Message);
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