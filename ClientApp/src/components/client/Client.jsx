import React, { useState } from 'react';
import MaterialTable from 'material-table';
import Alert from '@material-ui/lab/Alert';
import AddShoppingCartIcon from '@material-ui/icons/AddShoppingCart';

const baseURL = "api/Client";

const isOk = (response) => {
    if (response !== null && response.ok) {
        return response;
    } else {
        throw new Error(response.statusText);
    }
}

const columns =
    [
        { title: "id", field: "id", hidden: true },
        {
            title: 'Nome', field: 'name', type: 'string',
            validate: (rowData) => rowData.name === "" ?
                '⚠️ Nome deve ter entre 10 e 50 caracteres.' : ''
        },
        {
            title: 'Endereço', field: 'address', type: 'string',
            validate: rowData => rowData.address === ""
                ? '⚠️ Endereço deve ter entre 10 e 100 caracteres.' : ''
        },
        {
            title: 'Telefone', field: 'phoneNumber', type: 'string',
            validate: rowData => rowData.phoneNumber === ''
                ? '⚠️ Número de telefone deve ter 11 dígitos.' : ''
        },
        {
            title: 'Status', field: 'status', type: 'numeric',
            lookup: { 0: 'Ativo', 1: 'Inativo' }
        },
        {
            title: 'Débito', field: 'debt', type: 'currency', editable: 'never'
        },
        {
            title: 'Última compra', field: 'lastPurchase', type: 'date', editable: 'never'
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

export default function Client(props) {
    const [data, setData] = useState([]);
    const [errorMessages, setErrorMessages] = useState([]);
    const [iserror, setIserror] = useState(false);


    const handleRowAdd = (newData, resolve) => {
        fetch(baseURL, {
            method: 'Post',
            headers: { 'Content-type': 'application/json' },
            body: JSON.stringify(newData)
        })
            .then(res => isOk(res))
            .then(response => response.json())
            .then(product => {
                let dataToAdd = [...data];
                dataToAdd.push(product);
                setData(dataToAdd);
                resolve()
                setErrorMessages([])
                setIserror(false)
            })
            .catch(error => {
                setErrorMessages([`Não foi possível enviar os dados ao servidor. ${error}`])
                setIserror(true)
                resolve()
            })
    }

    const handleRowUpdate = (newData, oldData, resolve) => {
        newData.status = Number(newData.status);
        fetch(baseURL,
            {
                method: 'Put',
                headers: { 'Content-type': 'application/json' },
                body: JSON.stringify(newData)
            })
            .then(res => isOk(res))
            .then(response => response.json())
            .then(product => {
                const dataUpdate = [...data];
                const index = oldData.tableData.id;
                dataUpdate[index] = product;
                setData([...dataUpdate]);
                resolve()
                setIserror(false)
                setErrorMessages([])
            })
            .catch(error => {
                setErrorMessages(["Não foi possível atualizar o cliente. Erro no servidor."])
                setIserror(true)
                resolve()
            })

    }

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
                title="Clientes"
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
                        let url = 'api/Client?'
                        url += 'per_page=' + query.pageSize
                        url += '&page=' + (query.page + 1)
                        fetch(url)
                            .then(response => response.json())
                            .then(result => {
                                resolve({
                                    data: operations(query, result.data),
                                    page: result.page - 1,
                                    totalCount: result.total
                                })
                            }).catch(err => console.log(err))
                    })
                }
                actions={[
                    {
                        icon: () => <AddShoppingCartIcon />,
                        tooltip: 'Nova venda',
                        onClick: (event, rowData) => props.history.push(`/sale/client/${rowData.id}`)
                    }
                ]}
                editable={{
                    onRowAdd: newData =>
                        new Promise((resolve) => {
                            handleRowAdd(newData, resolve)
                        }),
                    onRowUpdate: (newData, oldData) =>
                        new Promise((resolve) => {
                            handleRowUpdate(newData, oldData, resolve);
                        }),
                }}
            />
        </>
    )
};

