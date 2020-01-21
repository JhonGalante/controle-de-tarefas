function validate() {
    $.ajax(
        {
            type: "POST",
            url: '@Url.Action("Index", "Home")',
            data: {
                email: $('#campoEmail').val(),
                senha: $('#campoSenha').val()
            }
        });
}
