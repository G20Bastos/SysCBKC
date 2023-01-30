const CroppieUtils = {
    createCroppie: (jquerySelector, jqueryButton) => {
         
        let basic = $(jquerySelector).croppie({
            enableExif: true,
            viewport: {
                width: 250,
                height: 250,
                type: 'circle'
            },
            boundary: { width: 300, height: 300 }
        });
        
        $(jqueryButton).click((event) => {
            event.preventDefault()
            $(jquerySelector).croppie('result', { type: 'base64', format: 'png' }).then(function (r) {              
                $("#lblImg64").val(r)
                $("#imgcabecalho").attr("src", r)
                $("#imgCrop").attr("src", r)
                $(".salve-crop").show(); 
            });
        })
        
    },
    updateCroppie: (jquerySelector, targetUrl) => {
        $(jquerySelector).croppie('bind', {
            url: targetUrl
        })
    }
}

