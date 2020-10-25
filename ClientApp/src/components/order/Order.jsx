import React, { useState } from 'react';
import MaterialTable from 'material-table';
import Alert from '@material-ui/lab/Alert';
import PaymentIcon from '@material-ui/icons/Payment';
import Items from './Items';

const columns =
    [
        { title: "id", field: "id", hidden: true },
        {
            title: 'Cliente', field: 'client.name', type: 'string'
        },
        {
            title: 'Valor', field: 'value', type: 'currency'
        },
        {
            title: 'Status', field: 'status', type: 'numeric',
            lookup: { 0: 'Pago', 1: 'Pendende' }
        },
        {
            title: 'Data', field: 'date', type: 'date'
        },
    ];

const localization = {
    body: {
        emptyDataSourceMessage: 'Nenhum registro para exibir',
        addTooltip: 'Adicionar',
        deleteTooltip: 'Apagar',
        editTooltip: 'Editar',
        editRow: {
            deleteText: 'Voulez-vous supprimer cette ligne?',
            cancelTooltip: 'Cancelar',
            saveTooltip: 'Salvar'
        }
    },
    toolbar: {
        searchTooltip: 'Pesquisar',
        searchPlaceholder: 'Pesquisar',
        exportTitle: 'Exportar',
        exportAriaLabel: 'Exportar',
    },
    pagination: {
        labelRowsSelect: 'linhas',
        labelDisplayedRows: '{count} de {from}-{to}',
        firstTooltip: 'Primeira página',
        previousTooltip: 'Página anterior',
        nextTooltip: 'Próxima página',
        lastTooltip: 'Última página'
    },
    header: {
        actions: 'Ações'
    }
}
const operations = (query, data) => {
    //Searching
    data = data.filter(p =>
        p.name.toLowerCase().includes(query.search.toLowerCase()) ||
        p.address.toLowerCase().includes(query.search.toLowerCase()) ||
        p.phoneNumber.includes(query.search)
    )
    //Sorting 
    if (query.orderBy != null) {
        let field = query.orderBy.field;
        data.sort(function (a, b) {
            if (a[field] > b[field]) {
                return 1;
            }
            if (a[field] < b[field]) {
                return -1;
            }
            // a must be equal to b
            return 0;
        });
    }
    return data;
};

export default function Order(props) {
    const [errorMessages, setErrorMessages] = useState([]);
    const [iserror, setIserror] = useState(false);

    return (
        <>
            <div>
                {iserror &&
                    <Alert
                        severity="error">
                        {errorMessages.map((msg, i) => {
                            return <div key={i}>{msg}</div>
                        })}
                    </Alert>
                }
            </div>
            <MaterialTable
                title="Pedidos"
                columns={columns}
                localization={localization}
                options={{
                    exportButton: true,
                    headerStyle: {
                        backgroundColor: '#01579b',
                        color: '#FFF'
                    }
                }}
                data={query =>
                    new Promise((resolve, reject) => {
                        let url = 'api/Order?'
                        url += 'per_page=' + query.pageSize
                        url += '&page=' + (query.page + 1)
                        fetch(url)
                            .then(response => response.json())
                            .then(result => {
                                resolve({
                                    data: result.data,
                                    page: result.page - 1,
                                    totalCount: result.total
                                })
                            }).catch(err => console.log(err))
                    })
                }
                actions={[
                    {
                        icon: () => <PaymentIcon />,
                        tooltip: 'Pagar',
                        onClick: (event, rowData) => props.history.push(`/sale/${rowData.id}`)
                    }
                ]}
                detailPanel={rowData => {
                    return (
                        <Items orderId={rowData.id} />
                    )
                }}
            />
        </>
    )
};

