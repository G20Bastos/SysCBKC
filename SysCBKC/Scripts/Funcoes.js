
//Formata campos de data, CPF e CNPJ
function Formatar(src, mask) {
    var i = src.value.length;
    var saida = mask.substring(0, 1);
    var texto = mask.substring(i)
    if (texto.substring(0, 1) != saida) {
	    src.value += texto.substring(0, 1);
    }
}

//Abre janela pop-up centralizada
function Centrar(arquivo, janela, largura, altura, scrollbar) {
	posx = (screen.width / 2) - (largura / 2);
	posy = (screen.height / 2) - (altura / 2);
	features = "width=" + largura + " ,height=" + altura + " ,top=" + posy + " ,left=" + posx + "," + scrollbar;
	window.open(arquivo, "", features);
}

//Verifica se e um numero valido
function numeroValido(src) {
    var tamanhoCampo = src.value.length - 1;
    var textoCampo = src.value;
    var qtdVirgula = 0;
    for (var i = 0; i < tamanhoCampo; i++) {
        if (textoCampo.substring(i, i + 1) == ",") {
            qtdVirgula = qtdVirgula + 1;
        }
        if (!(textoCampo.substring(i, i + 1) >= 0) && (textoCampo.substring(i, i + 1) <= 9)) {
            return false;
        }
    }
    if (qtdVirgula > 1) {
        return false;
    }
    return true;
}

//Controla a visibilidade das divs do PreProcessoEditar
function expandirDivPreProcesso(opcao){

    //Digitalização
    if (opcao == 1) {
        var element = document.getElementById('divDigitalizacao');
        var element2 = document.getElementById('btnExapandirDigitalizacao');
        var isVisible = (element.offsetWidth > 0 || element.offsetHeight > 0);

        if (isVisible == true) {
            element2.innerText = '[+]'
            element.style.display = 'none'
        } else {
            element2.innerText = '[-]'
            element.style.display = 'block'
        }
    }

    //Síntese e Parecer
    if (opcao == 2) {
        var element = document.getElementById('divSinteseParecer');
        var element2 = document.getElementById('btnExpandirSinteseParecer');
        var isVisible = (element.offsetWidth > 0 || element.offsetHeight > 0);

        if (isVisible == true) {
            element2.innerText = '[+]'
            element.style.display = 'none'
        } else {
            element2.innerText = '[-]'
            element.style.display = 'block'
        }
    }

    //Redação
    if (opcao == 3) {
        var element = document.getElementById('divRedacao');
        var element2 = document.getElementById('btnExpandirRedacao');
        var isVisible = (element.offsetWidth > 0 || element.offsetHeight > 0);

        if (isVisible == true) {
            element2.innerText = '[+]'
            element.style.display = 'none'
        } else {
            element2.innerText = '[-]'
            element.style.display = 'block'
        }
    }

    //Revisão
    if (opcao == 4) {
        var element = document.getElementById('divRevisao');
        var element2 = document.getElementById('btnExpandirRevisao');
        var isVisible = (element.offsetWidth > 0 || element.offsetHeight > 0);

        if (isVisible == true) {
            element2.innerText = '[+]'
            element.style.display = 'none'
        } else {
            element2.innerText = '[-]'
            element.style.display = 'block'
        }
    }

    //Montagem
    if (opcao == 5) {
        var element = document.getElementById('divMontagem');
        var element2 = document.getElementById('btnExpandirMontagem');
        var isVisible = (element.offsetWidth > 0 || element.offsetHeight > 0);

        if (isVisible == true) {
            element2.innerText = '[+]'
            element.style.display = 'none'
        } else {
            element2.innerText = '[-]'
            element.style.display = 'block'
        }
    }

    //Diligência
    if (opcao == 6) {
        var element = document.getElementById('divDiligencia');
        var element2 = document.getElementById('btnExpandirDiligencia');
        var isVisible = (element.offsetWidth > 0 || element.offsetHeight > 0);

        if (isVisible == true) {
            element2.innerText = '[+]'
            element.style.display = 'none'
        } else {
            element2.innerText = '[-]'
            element.style.display = 'block'
        }
    }

    //Protocolo
    if (opcao == 7) {
        var element = document.getElementById('divProtocolo');
        var element2 = document.getElementById('btnExpandirProtocolo');
        var isVisible = (element.offsetWidth > 0 || element.offsetHeight > 0);

        if (isVisible == true) {
            element2.innerText = '[+]'
            element.style.display = 'none'
        } else {
            element2.innerText = '[-]'
            element.style.display = 'block'
        }
    }

    //Investigação
    if (opcao == 8) {
        var element = document.getElementById('divInvestigacao');
        var element2 = document.getElementById('btnExpandirInvestigacao');
        var isVisible = (element.offsetWidth > 0 || element.offsetHeight > 0);

        if (isVisible == true) {
            element2.innerText = '[+]'
            element.style.display = 'none'
        } else {
            element2.innerText = '[-]'
            element.style.display = 'block'
        }
    }

    //Denuncia
    if (opcao == 9) {
        var element = document.getElementById('divDenuncia');
        var element2 = document.getElementById('btnExpandirDenuncia');
        var isVisible = (element.offsetWidth > 0 || element.offsetHeight > 0);

        if (isVisible == true) {
            element2.innerText = '[+]'
            element.style.display = 'none'
        } else {
            element2.innerText = '[-]'
            element.style.display = 'block'
        }
    }

    //Processo
    if (opcao == 10) {
        var element = document.getElementById('divProcesso');
        var element2 = document.getElementById('btnExpandirProcesso');
        var isVisible = (element.offsetWidth > 0 || element.offsetHeight > 0);

        if (isVisible == true) {
            element2.innerText = '[+]'
            element.style.display = 'none'
        } else {
            element2.innerText = '[-]'
            element.style.display = 'block'
        }
    }

    //Recurso / Cumprimento de Pena
    if (opcao == 11) {
        var element = document.getElementById('divRecursoCumprimentoPena');
        var element2 = document.getElementById('btnExpandirRecursoCumprimentoPena');
        var isVisible = (element.offsetWidth > 0 || element.offsetHeight > 0);

        if (isVisible == true) {
            element2.innerText = '[+]'
            element.style.display = 'none'
        } else {
            element2.innerText = '[-]'
            element.style.display = 'block'
        }
    }

    //Recurso
    if (opcao == 12) {
        var element = document.getElementById('divRecurso');
        var element2 = document.getElementById('btnExpandirRecurso');
        var isVisible = (element.offsetWidth > 0 || element.offsetHeight > 0);

        if (isVisible == true) {
            element2.innerText = '[+]'
            element.style.display = 'none'
        } else {
            element2.innerText = '[-]'
            element.style.display = 'block'
        }
    }

    //Cumprimento da Pena
    if (opcao == 13) {
        var element = document.getElementById('divCumprimentoPena');
        var element2 = document.getElementById('btnExpandirCumprimentoPena');
        var isVisible = (element.offsetWidth > 0 || element.offsetHeight > 0);

        if (isVisible == true) {
            element2.innerText = '[+]'
            element.style.display = 'none'
        } else {
            element2.innerText = '[-]'
            element.style.display = 'block'
        }
    }

}

//Filtra a lista de nomes em um CheckListBox
function filtraNomesCheckListBox(checkListBox, textbox) {

    var tmr = false;
    var labels = document.getElementById(checkListBox).getElementsByTagName('label');

    var func = function () {
        if (tmr)
            clearTimeout(tmr);
        tmr = setTimeout(function () {
            var regx = new RegExp(document.getElementById(textbox).value.toLowerCase());
            
            for (var i = 0, size = labels.length; i < size; i++)
                if (document.getElementById(textbox).value.length > 0) {

                    if (labels[i].textContent != null) {
                        if (labels[i].textContent.toLowerCase().match(regx)) setItemVisibility(i, true);
                        else setItemVisibility(i, false);
                    } 
                    
                }
                else
                    setItemVisibility(i, true);
        }, 500);

        function setItemVisibility(position, visible) {
            if (visible) {
                labels[position].style.display = '';
                labels[position].previousSibling.style.display = '';
                if (labels[position].nextSibling != null)
                    labels[position].nextSibling.style.display = '';
            }
            else {
                labels[position].style.display = 'none';
                labels[position].previousSibling.style.display = 'none';
                if (labels[position].nextSibling != null)
                    labels[position].nextSibling.style.display = 'none';
            }
        }
    }

    if (document.attachEvent) document.getElementById(textbox).attachEvent('onkeypress', func);  // IE compatibility
    else document.getElementById(textbox).addEventListener('keydown', func, false); // other browsers

};
//Filtra a lista de nomes em um ListBox
function filtraNomesListBox(dpr, textbox) {
    
    var tmr = false;
    var labels = document.getElementById(dpr).getElementsByTagName('option');

    var func = function () {
        if (tmr)
            clearTimeout(tmr);
        tmr = setTimeout(function () {
            var regx = new RegExp(document.getElementById(textbox).value.toLowerCase());

            for (var i = 0, size = labels.length; i < size; i++)
                if (document.getElementById(textbox).value.length > 0) {

                    if (labels[i].textContent != null) {
                        if (labels[i].textContent.toLowerCase().match(regx)) setItemVisibility(i, true);
                        else setItemVisibility(i, false);
                    }

                }
                else
                    setItemVisibility(i, true);
        }, 500);

        function setItemVisibility(position, visible) {
           
            if (visible) {
                labels[position].style.display = "block";
                labels[position].style.webkitTransform = "appearance(block)";
             
            }
            else {
                labels[position].style.display = "none";
                labels[position].style.webkitTransform = "appearance(none)";
            }
        }
    }

    if (document.attachEvent) document.getElementById(textbox).attachEvent('onkeypress', func);  // IE compatibility
    else document.getElementById(textbox).addEventListener('keydown', func, false); // other browsers
   }