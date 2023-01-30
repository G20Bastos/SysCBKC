$(document).ready(function () {
    $(".result").hide();
    $(".salve-crop").hide();
    if ($("#lblImg64").val() == "../Imagens/Avatar_Perfil.jpg") {
        $(".exclui-crop").hide();
    }
    if ($("#lblImg64").val() == "") {
        $(".exclui-crop").hide();
    }
    $(document).on('change', '.btn-file :file', function () {
        var input = $(this),
            label = input.val().replace(/\\/g, '/').replace(/.*\//, '');
        input.trigger('fileselect', [label]);
    });

    $('.btn-file :file').on('fileselect', function (event, label) {
        $(".croppie-container").show();
        var input = $(this).parents('.input-group').find(':text'),
            log = label;

        if (input.length) {
            input.val(log);
        } else {
            if (log) alert(log);
        }

    });
    function readURL(input) {
       
        if (input.files && input.files[0]) {
            var reader = new FileReader();
            
            reader.onload = function (e) {
                if (!$('.my-image').hasClass('cr-original-image')) {
                    CroppieUtils.createCroppie($('.my-image'), $(".result"))
                }
                CroppieUtils.updateCroppie($('.my-image'), e.target.result)
               
                $(".result").show();
                $("#excluirCrop").hide()
            }
           
            reader.readAsDataURL(input.files[0]);
        }
    }
    $("#imgInp").change(function () {
        readURL(this);
    });
    $("#salvarCrop").click(function () {
        event.preventDefault();
        $(".croppie-container").hide(); 
        $(".result").hide();
        $(".salve-crop").hide();
        $("#imgInp").val(null);
        if ($("#lblImg64").val() != "../Imagens/Avatar_Perfil.jpg") {
            $(".exclui-crop").show();
        } 
    });
    $("#excluirCrop").click(function () {
        event.preventDefault();
        $("#lblImg64").val("../Imagens/Avatar_Perfil.jpg")
        $("#imgCrop").attr("src", "../Imagens/Avatar_Perfil.jpg")
        $(".exclui-crop").hide();
        $("#inputTitulo").val(null);
    });
});

