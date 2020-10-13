import React, { useState } from 'react';
import { makeStyles } from '@material-ui/core/styles';
import Grid from '@material-ui/core/Grid';
import Alert from '@material-ui/lab/Alert';

import { TableItem } from './TableItem';
import { TabItem } from './TabItem';

const baseURL = "";

const isOk = (response) => {
    if (response !== null && response.ok) {
        return response;
    } else {
        throw new Error(response.statusText);
    }
}


export default function Sale(props) {
    const [errorMessages, setErrorMessages] = useState([]);
    const [iserror, setIserror] = useState(false);
    const [products, setProducts] = useState([]);
    const clientId = props.match.params.clientId;

    const useStyles = makeStyles((theme) => ({
        root: {
            flexGrow: 1,
        },
        paper: {
            padding: theme.spacing(2),
            textAlign: 'center',
            color: theme.palette.text.secondary,
        },
    }));

    const onAdd = (product) => {
        setProducts([...products, product]);
    }

    const handleSaveItems = () => {
        let order = {
            clientId: clientId,
            items: products.map(p => ({ 'ProductId': p.productid, 'Quantity': p.quantity }))
        }
        fetch(baseURL, {
            method: 'Post',
            headers: { 'Content-type': 'application/json' },
            body: JSON.stringify(order)
        })
            .then(response => isOk(response))
            .then(response => response.json())
            .then(result => {
                setErrorMessages([])
                setIserror(false)
            })
            .catch(error => {
                setErrorMessages([`Não foi possível enviar os dados ao servidor. ${error}`])
                setIserror(true)
            })
    }
    const handleRowUpdate = (newData, oldData, resolve) => {
        let objIndex = products.findIndex(p => p.code == oldData.code);
        let productscopy = [...products];
        productscopy[objIndex] = newData;
        setProducts(productscopy);
        resolve();
    }

    const handleRowDelete = (oldData, resolve) => {
        let newproducts = products.filter(p => p.code != oldData.code);
        setProducts(newproducts);
        resolve();
    }

    const classes = useStyles();
    return (<>
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
        <div className={classes.root}>
            <Grid container spacing={3}>
                <Grid item xs={8}>
                    <TableItem products={products}
                        onRowUpdate={handleRowUpdate}
                        onRowDelete={handleRowDelete}
                        onSaveItems={handleSaveItems}
                    />
                </Grid>
                <Grid item xs={4}>
                    <TabItem onAdd={onAdd} />
                </Grid>
            </Grid>
        </div>
    </>
    );

};



