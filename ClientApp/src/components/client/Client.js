import React, { useState, useEffect } from 'react';
import MaterialTable from 'material-table';
import Alert from '@material-ui/lab/Alert';
import { Select, MenuItem } from "@material-ui/core";

const baseURL = "api/Client";

function renderProductsTable(clients, handleRowAdd, handleRowUpdate, iserror, errorMessages) {
    const status = ["Ativo", "Inativo"];
    const columns =
        [
            { title: "id", field: "id", hidden: true },
            {
                title: 'Nome', field: 'name',
                validate: rowData => rowData.name === '' ? 'Nome não pode ser vazio.' : ''
            },
            {
                title: 'Endereço', field: 'address',
                validate: rowData => (rowData.address === '' || rowData.address.length < 10 || rowData.address.length > 100)
                    ? 'Endereço deve ter entre 10 e 100 caracteres.' : ''
            },
            {
                title: 'Telefone', field: 'phoneNumber',
                validate: rowData => (rowData.address === '' || rowData.phoneNumber !== 11)
                    ? 'Número de telefone deve ter 11 caracteres.' : ''
            },
            {
                title: 'Débito', field: 'debt', type: 'numeric', editable: 'never'
            },
            {
                title: 'Status', field: 'status',
                editComponent: ({ value, onRowDataChange, rowData }) => (
                    <Select>
                        {status.map((state) => (
                            <MenuItem key={state} value={state}>
                                {state}
                            </MenuItem>
                        ))}
                    </Select>
                )
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
        }
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
                title="Clients"
                data={clients}
                columns={columns}
                localization={localization}
                options={{ exportButton: true }}
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



function Client() {

    const [data, setData] = useState([]);
    const [errorMessages, setErrorMessages] = useState([]);
    const [iserror, setIserror] = useState(false);
    const [isloading, setIsLoading] = useState(true);

    const isOk = (response) => {
        if (response !== null && response.ok) {
            return response;
        } else {
            throw new Error(response.statusText);
        }
    }

    useEffect(() => {
        fetch(baseURL)
            .then(res => isOk(res))
            .then(response => response.json())
            .then(data => {
                setData(data);
                setIsLoading(false);
            })
            .catch(err => console.log(err));
    }, [])


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

    return (isloading ?
        <p><em>Carregando...</em></p> :
        renderProductsTable(data, handleRowAdd, handleRowUpdate, iserror, errorMessages));

};

export default Client;