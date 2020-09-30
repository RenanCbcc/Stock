import React, { useState } from 'react';
import { makeStyles } from '@material-ui/core/styles';
import TextField from '@material-ui/core/TextField';
import Button from '@material-ui/core/Button';

const baseURL = "api/Product/Code?code=";

const useStyles = makeStyles((theme) => ({
    text: {
        '& > *': {
            margin: theme.spacing(1),
            width: '40ch',
        },
    },
    button: {
        '& > *': {
            margin: theme.spacing(1),
        },
    },
}));


export function AutomaticTabPanel() {
    const [code, setCode] = useState('');

    const classes = useStyles();

    const isOk = (response) => {
        if (response !== null && response.ok) {
            return response;
        } else {
            throw new Error(response.statusText);
        }
    }

    const onClick = () => {
        
        return fetch(`api/Product/Code?code=7896591527269`, { method: 'GET' })
            .then(res => isOk(res))
            .then(res => res.json())
            .then((product) => console.log(product))
            .catch(err => { throw new Error(err) });
    }

    return (
        <form className={classes.text} noValidate autoComplete="off">
            <TextField fullWidth required id="code" label="Código" variant="outlined" />
            <TextField disabled id="description" label="Descrição" variant="outlined" />
            <TextField disabled id="price" label="Preço" variant="outlined" />
            <TextField required type="number" id="quantity" label="Quantidade" variant="outlined" />

            <div className={classes.button}>
                <Button variant="contained" onClick={onClick} >Buscar</Button>
                <Button variant="contained" color="primary">Adicionar</Button>
            </div>
        </form>
    )
}