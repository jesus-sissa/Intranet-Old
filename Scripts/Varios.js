/// <reference name="MicrosoftAjax.js"/>

     <script type="text/javascript" language="javascript">

        function SelectAllCheckboxes(spanChk) {

            // Added as ASPX uses SPAN for checkbox
            var oItem = spanChk.children;
            var theBox = (spanChk.type == "checkbox") ?
            spanChk : spanChk.children.item[0];
            xState = theBox.checked;
            elm = theBox.form.elements;

            for (i = 0; i < elm.length; i++)
                if (elm[i].type == "checkbox" &&
              elm[i].id != theBox.id) {
                //elm[i].click();
                if (elm[i].checked != xState)
                    elm[i].click();
                //elm[i].checked=xState;
            }
        }
    </script>

    <script type="text/javascript">
        function CopyCheckStateByColumn(HeaderCheckBox, gridViewName) {
            var columnIndex = HeaderCheckBox.parentElement.cellIndex;
            var newState = HeaderCheckBox.checked;
            ChangeChecksByColumn(gridViewName, newState, columnIndex);
        }
    </script>

    <script type="text/javascript">
        function ChangeChecksByColumn(gridViewName, newState, columnIndex) {
            var tabla = document.getElementById(gridViewName);
            var columnas = tabla.cells.length / tabla.rows.length;
            celdas = tabla.cells;
            for (i = columnas + columnIndex; i < celdas.length; i += columnas) {
                if (celdas[i].firstChild.type == "checkbox"
               && celdas[i].firstChild.checked != newState
                /* && agregar aquí otras condiciones */) {
                    celdas[i].firstChild.click();
                }
            }
        }
    </script>

