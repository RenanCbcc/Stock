import React, { useState } from 'react';
import { makeStyles } from '@material-ui/core/styles';
import TextField from '@material-ui/core/TextField';
import Button from '@material-ui/core/Button';

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


export function AutomaticTabPanel(props) {
    const [code, setCode] = useState('');
    const [description, setDescription] = useState('');
    const [price, setPrice] = useState('');
    const [quantity, setQuantity] = useState('');
    const [codeerror, setCodeError] = useState({ code: { valid: true, text: "" } });
    const [quantityerror, setQuantityErrors] = useState({ quantity: { valid: true, text: "" } });
    const classes = useStyles();

    const isOk = (response) => {
        if (response !== null && response.ok) {
            return response;
        } else {
            throw new Error(response.statusText);
        }
    }

    const onClick = () => {
        //7896591527269
        return fetch(`api/Product/Code?code=${code}`, { method: 'GET' })
            .then(res => isOk(res))
            .then(res => res.json())
            .then((product) => {
                setDescription(product.description);
                setPrice(product.salePrice);
            })
            .catch(err => { throw new Error(err) });
    }

    const onSubmit = (event) => {
        event.preventDefault();
        props.onAdd({ code, description, price, quantity, subtotal: price * quantity });

        setCode('');
        setDescription('');
        setPrice('');
        setQuantity('');
    }

    return (
        <form className={classes.text} autoComplete="off"
            onSubmit={onSubmit}>

            <TextField
                fullWidth required
                id="code"
                value={code}
                label="Código"
                variant="outlined"
                error={!codeerror.code.valid}
                helperText={codeerror.code.text}
                onChange={(event) => {
                    let c = event.target.value;
                    if (c.length < 9 || c.length > 13) {
                        setCodeError(
                            {
                                code:
                                {
                                    valid: false,
                                    text: "O código precisa ter entre 9 e 13 dígitos."
                                }
                            });
                    } else {
                        setCodeError({ code: { valid: true, text: "" } });
                    }
                    setCode(c);
                }}
            />
            <TextField
                disabled id="description"
                value={description}
                label="Descrição"
                variant="outlined"
            />
            <TextField
                disabled id="price"
                value={price}
                label="Preço"
                variant="outlined"
            />
            <TextField
                required type="number"
                id="quantity"
                label="Quantidade"
                variant="outlined"
                value={quantity}
                error={!quantityerror.quantity.valid}
                helperText={quantityerror.quantity.text}
                onChange={(event) => {
                    let q = event.target.value;
                    if (q <= 0) {
                        setQuantityErrors(
                            {
                                quantity:
                                {
                                    valid: false,
                                    text: "A quantidade precisa ser maior que 0."
                                }
                            });
                    } else {
                        setQuantityErrors({ quantity: { valid: true, text: "" } });
                    }
                    setQuantity(q);
                }}
            />

            <div className={classes.button}>
                <Button variant="contained" onClick={onClick}>Buscar</Button>
                <Button type="submit" variant="contained" color="primary">Adicionar</Button>
            </div>
        </form>
    )
}