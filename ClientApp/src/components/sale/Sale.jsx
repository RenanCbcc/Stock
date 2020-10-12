import React, { useState } from 'react';
import { makeStyles } from '@material-ui/core/styles';
import Grid from '@material-ui/core/Grid';
import Alert from '@material-ui/lab/Alert';

import { TableItem } from './TableItem';
import { TabItem } from './TabItem';


export function Sale() {
    const [errorMessages, setErrorMessages] = useState([]);
    const [iserror, setIserror] = useState(false);
    const [products, setProducts] = useState([]);

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
                        onRowDelete={handleRowDelete} />
                </Grid>
                <Grid item xs={4}>
                    <TabItem onAdd={onAdd} />
                </Grid>
            </Grid>
        </div>
    </>
    );

};



