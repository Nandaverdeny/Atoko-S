$(document).ready(function () {

        $('#data').DataTable();

        $('#data1').DataTable({
            "footerCallback": function (row, data, start, end, display) {
                var api = this.api(), data;

                // Remove the formatting to get integer data for summation
                var intVal = function (i) {
                    return typeof i === 'string' ?
                        i.replace(/[\$,]/g, '') * 1 :
                        typeof i === 'number' ?
                        i : 0;
                };

                // Total over all pages
                total = api
                    .column(4)
                    .data()
                    .reduce(function (a, b) {
                        return intVal(a) + intVal(b);
                    }, 0);
                //var col4 = $('#mytable').dataTable()._('td:nth-child(4)', { "filter": "applied" });
                //var totFilter = 0;
                //for (i = 0; i < col4.length; i++) {
                //    totFilter = col4[i]++;
                //}
                //total = totFilter;
                // Total over this page
                pageTotal = api
                    .column(4, { page: 'current' })
                    .data()
                    .reduce(function (a, b) {
                        return intVal(a) + intVal(b);
                    }, 0);

                // Update footer
                $(api.column(4).footer()).html(
                    pageTotal + ' ( ' + total + ' total)'
                );
            }
        }

            );


    });