import React, { useState, useEffect } from 'react';
import MaterialTable from 'material-table';
import Alert from '@material-ui/lab/Alert';

const baseURL = "api/Product";

function renderProductsTable(products, handleRowAdd, handleRowUpdate, iserror, errorMessages) {
    const columns =
        [
            { title: "id", field: "id", hidden: true },
            { title: 'Descrição', field: 'description' },
            { title: 'Código', field: 'code' },
            { title: 'Preço de compra', field: 'purchasePrice', type: 'numeric' },
            { title: 'Preço de venda', field: 'salePrice', type: 'numeric' },
            { title: 'Quantidade', field: 'quantity', type: 'numeric' },
            { title: 'Desconto', field: 'discount', type: 'numeric' },
        ];

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
                data={products}
                columns={columns}
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
        //validation
        let errorList = []

        if (newData.description === undefined) {
            errorList.push("O produto precisa ter uma descrição.")
        }

        if (newData.code === undefined) {
            errorList.push("O produto precisa ter um código.")
        }
        if (newData.purchasePrice === undefined) {
            errorList.push("O produto precisa ter um valor de preço de compra.")
        }

        if (newData.purchasePrice === undefined) {
            errorList.push("O produto precisa ter um valor de preço de compra.")
        }

        if (newData.salePrice === undefined) {
            errorList.push("O produto precisa ter um valor de preço de venda.")
        }

        if (newData.quantity === undefined) {
            errorList.push("O produto precisa ter uma quantidade.")
        }


        if (errorList.length === 0) { //no error
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
        } else {
            console.log(errorList)
            setErrorMessages(errorList)
            setIserror(true)
            resolve()
        }


    }

    const handleRowUpdate = (newData, oldData, resolve) => {
        //validation
        let errorList = []

        if (newData.description === undefined) {
            errorList.push("O produto precisa ter uma descrição.")
        }

        if (newData.purchasePrice === undefined) {
            errorList.push("O produto precisa ter um valor de preço de compra.")
        }

        if (newData.purchasePrice === undefined) {
            errorList.push("O produto precisa ter um valor de preço de compra.")
        }

        if (newData.salePrice === undefined) {
            errorList.push("O produto precisa ter um valor de preço de venda.")
        }

        if (newData.quantity === undefined) {
            errorList.push("O produto precisa ter uma quantidade.")
        }

        if (errorList.length === 0) {
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
        } else {
            setErrorMessages(errorList)
            setIserror(true)
            resolve()

        }

    }

    return (isloading ?
        <p><em>Carregando...</em></p> :
        renderProductsTable(data, handleRowAdd, handleRowUpdate, iserror, errorMessages));

};

export default Product;