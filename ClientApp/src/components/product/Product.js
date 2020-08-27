import React, { useState, useEffect } from 'react';
import MaterialTable from 'material-table';
import Alert from '@material-ui/lab/Alert';

const baseURL = "api/Product";

function renderProductsTable(handleRowAdd, handleRowUpdate, iserror, errorMessages) {
    const columns =
        [
            { title: "id", field: "id", hidden: true },
            {
                title: 'Descrição', field: 'description', type: 'string',
                validate: rowData => rowData.description.length < 10 || rowData.description.length > 50
                    ? '⚠️ Descrição não pode ser vazia' : ''
            },
            {
                title: 'Código', field: 'code', type: 'string',
                validate: rowData => rowData.code.length < 9 || rowData.code.length > 13
                    ? '⚠️ Código deve ter entre 9 e 13 dígitos' : ''
            },
            {
                title: 'Preço de compra', field: 'purchasePrice', type: 'currency',
                validate: rowData => rowData.purchasePrice < 0 ? '⚠️ Preço de compra não pode ser menor que zero' : ''
            },
            {
                title: 'Preço de venda', field: 'salePrice', type: 'currency',
                validate: rowData => rowData.salePrice < 0 ? '⚠️ Preço de venda não pode ser menor que zero' : ''
            },
            {
                title: 'Quantidade', field: 'quantity', type: 'numeric',
                validate: rowData => rowData.quantity < 0 ? '⚠️ Quantidade não pode ser menor que zero' : ''
            },
            {
                title: 'Desconto', field: 'discount', type: 'numeric',
                validate: rowData => (rowData.discount < 0 || rowData.discount > 100) ? '⚠️ Desconto deve ser >= 0 e <=100' : ''
            },
            {
                title: 'Categoria', field: 'categoryId', type: 'numeric',
                lookup: { 1: 'Limpeza', 2: 'Roupas', 3: 'Sapatos' }
            },
            {
                title: 'Fornecedor', field: 'supplierId', type: 'numeric',
                lookup: { 1: 'Mariza', 2: 'Havainas' }
            }
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
                title="Produtos"
                columns={columns}
                localization={localization}
                options={{ exportButton: true }}
                data={query =>
                    new Promise((resolve, reject) => {
                        fetch(baseURL)
                            .then(response => response.json())
                            .then(result => {
                                resolve({
                                    data: result
                                })
                            }).catch(err => console.log(err))
                    })
                }
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



function Product() {

    const [data, setData] = useState([]);
    const [categories, setCategories] = useState([]);
    const [suppliers, setsuppliers] = useState([]);
    const [errorMessages, setErrorMessages] = useState([]);
    const [iserror, setIserror] = useState(false);

    const isOk = (response) => {
        if (response !== null && response.ok) {
            return response;
        } else {
            throw new Error(response.statusText);
        }
    }


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
                setErrorMessages(["Não foi possível atualizar o produto. Erro no servidor."])
                setIserror(true)
                resolve()
            })

    }

    return (renderProductsTable(handleRowAdd, handleRowUpdate, iserror, errorMessages));

};

export default Product;