/// <reference path="../lib/jquery/dist/jquery.js" />
/// <reference path="datagrid/datatables/datatables.bundle.js" />

export function showPrompt() {
    return prompt('Finally worked Yaay', 'Type anything here');
}

export function initializeJobOrdersDataTable(dataObjectId, dataTypeId) {
    $("#dt-all-orders-component").dataTable(
        {
            processing: true,
            serverSide: true,
            ajax: {
                contentType: "application/json; charset=utf-8",
                url: "/api/components/GetAllJobOrdersDataTableComponentData",
                type: 'POST',
                data: function (d) {
                    return JSON.stringify({ parameters: d, dataObjectId, dataTypeId });
                }
            },
            searching: false,
            columns: [
                { "data": "jobOrderId" },
                {
                    "data": "enquiryId",
                    render: function (data, type, row, meta) {
                        if (type === "display") {
                            if (data == 0) {
                                return 'N/A';
                            }
                            else {
                                let link = `https://myplusbeta.publicisgroupe.net/enquiry?ID=${row.enquiryId}`;
                                return '<a href="' + link + '" target="_blank" >' + data + '</a>';
                            }
                        }
                        return data;
                    }
                },
                {
                    "data": "jobOrderName",
                    render: function (data, type, row, meta) {
                        if (type === "display") {
                            let link = `https://myplusbeta.publicisgroupe.net/tmsjoborder/joborder?joborderid=${row.jobOrderId}`;

                            return '<a href="' + link + '" target="_blank" >' + data + '</a>';
                        }
                        return data;
                    }
                },
                {
                    "data": "orgName",
                    render: function (data, type, row, meta) {
                        if (type === "display") {
                            let link = `http://myplus/Org.aspx?OrgID=${row.orgId}`;

                            return '<a href="' + link + '" target="_blank" >' + data + '</a>';
                        }
                        return data;
                    }
                },
                {
                    "data": "orgGroupName",
                    render: function (data, type, row, meta) {
                        if (type === "display") {
                            let link = `http://myplus/OrgGroup.aspx?GroupID=${row.orgGroupId}`;

                            return '<a href="' + link + '" target="_blank" >' + data + '</a>';
                        }
                        return data;
                    }
                },
                {
                    "data": "contactName",
                    render: function (data, type, row, meta) {
                        if (type === "display") {
                            let link = `http://myplus/Contact.aspx?ContactID=${row.contactId}`;

                            return '<a href="' + link + '" target="_blank" >' + data + '</a>';
                        }
                        return data;
                    }
                },
                { "data": "sourceLang" },
                { "data": "targetLang" },
                { "data": "status" },
                { "data": "submittedDate" },
                { "data": "deliveryDeadline" },
                {
                    "data": "margin",
                    render: function (data, type, row, meta) {
                        if (type === "display") {
                            return `${data.toFixed(2)} %`;
                        }
                        return data;
                    }
                },
                {
                    "data": "value",
                    render: function (data, type, row, meta) {
                        if (type === "display") {
                            return `<i>${row.currency}</i>  ${Number(data.toFixed(2)).toLocaleString()}`;
                        }
                        return data;
                    }
                }
            ],
            fixedHeader: true,
            colReorder: true,
            responsive: true,
            dom:
                "<'row mb-3'<'col-sm-12 col-md-6 d-flex align-items-center justify-content-start'f><'col-sm-12 col-md-6 d-flex align-items-center justify-content-end'B>>" +
                "<'row'<'col-sm-12'tr>>" +
                "<'row'<'col-sm-12 col-md-5'i><'col-sm-12 col-md-7'p>>",
            buttons: [
                {
                    extend: 'pageLength',
                    className: 'btn-outline-default'
                },
                {
                    extend: 'colvis',
                    text: 'Column Visibility',
                    titleAttr: 'Col visibility',
                    className: 'btn-outline-default'
                }
            ]
        });
}