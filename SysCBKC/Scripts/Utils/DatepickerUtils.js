const DatepickerUtils = {
    createDatepicker: (jquerySelector) => {
        if ($(jquerySelector).is(':enabled')) {
            $(jquerySelector).datepicker({
                uiLibrary: 'bootstrap4',
                format: 'dd/mm/yyyy',
                locale: 'pt-br',
                header: true
            });

            //função para gerar pegar a data atual 
            var str_data
            // Obtém a data/hora atual
            var data = new Date();

            // Guarda cada pedaço em uma variável
            var dia = data.getDate();           // 1-31
            if ((dia) < "10") {
                dia = '0' + dia
            }
            var dia_sem = data.getDay();            // 0-6 (zero=domingo)
            var mes = data.getMonth();          // 0-11 (zero=janeiro)
            var ano2 = data.getYear();           // 2 dígitos
            var ano4 = data.getFullYear();       // 4 dígitos

            // Formata a data e a hora (note o mês + 1)
            if ((mes) < "10") {
                str_data = dia + '/' + '0' + (mes + 1) + '/' + ano4;
            }
            if ((mes) > "9") {
                str_data = dia + '/' + (mes + 1) + '/' + ano4;
            }

            //aqui é para assim que ele clicar no calendário, mostra a data de hoje no cabeçalho
            $(".input-group-append").click(function () {
                $("[role=date]").html('Hoje: ' + str_data)
                $("[role=year]").html(" ")
            });

            $(".gj-picker").click(function () {
                $("[role=date]").html('Hoje: ' + str_data)
                $("[role=year]").html(" ")
            });

            //Ao clicar no cabeçalho, o valor da data atual vai para o input
            let guid = $(jquerySelector).attr('data-guid')
            $("div[guid='" + guid + "']").click(function () {
                $(jquerySelector).val(str_data);
            });

            $(".selected").click(function () {
                $(".gj-picker").hide();
            });

            $(jquerySelector).unbind("focus")
        }
    } 
}