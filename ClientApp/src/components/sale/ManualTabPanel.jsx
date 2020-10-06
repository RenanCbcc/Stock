import React, { useState, useEffect } from 'react';
import { makeStyles } from '@material-ui/core/styles';
import InputLabel from '@material-ui/core/InputLabel';
import MenuItem from '@material-ui/core/MenuItem';
import FormControl from '@material-ui/core/FormControl';
import Select from '@material-ui/core/Select';
import TextField from '@material-ui/core/TextField';
import Button from '@material-ui/core/Button';

const useSelectStyles = makeStyles((theme) => ({
    formControl: {
        margin: theme.spacing(1),
        minWidth: 120,
        width: '40ch',
    },
    selectEmpty: {
        marginTop: theme.spacing(2),
    },
}));

const useInputStyles = makeStyles((theme) => ({
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

export function ManualTabPanel(props) {
    const [categories, setCategories] = useState([]);
    const [products, setProducts] = React.useState([]);
    const [price, setPrice] = useState('');
    const [quantity, setQuantity] = useState('');
    const [quantityerror, setQuantityErrors] = useState({ quantity: { valid: true, text: "" } });

    const handleCategoryChange = (event) => {
        console.log(event.target.value)
    };

    const handleProductChange = (event) => {
        
    };

    const selectStyles = useSelectStyles();
    const inputStyles = useInputStyles();

    const onSubmit = (event) => {
        event.preventDefault();
        //props.onAdd({ code, description, price, quantity });

        setPrice('');
        setQuantity('');
    }

    const isOk = (response) => {
        if (response !== null && response.ok) {
            return response;
        } else {
            throw new Error(response.statusText);
        }
    }

    useEffect(() => {
        fetch('https://localhost:44308/api/Category/All')
            .then(res => isOk(res))
            .then(response => response.json())
            .then(data => { setCategories(data) })
            .catch(err => console.log(err));
    }, [])

    return (
        <form className={inputStyles.text} autoComplete="off"
            onSubmit={onSubmit}>

            <FormControl variant="outlined" className={selectStyles.formControl}>
                <InputLabel id="demo-simple-select-label">Categoria</InputLabel>
                <Select
                    labelId="demo-simple-select-label"
                    id="demo-simple-select"
                    value={categories}
                    onChange={handleCategoryChange}
                >
                    <MenuItem value="">
                        <em>None</em>
                    </MenuItem>

                    {categories.map((category) =>
                        <MenuItem value={category.id}>{category.title}</MenuItem>
                    )}

                </Select>
            </FormControl>
            <FormControl variant="outlined" className={selectStyles.formControl}>
                <InputLabel id="demo-simple-select-label">Produto</InputLabel>
                <Select
                    labelId="demo-simple-select-label"
                    id="demo-simple-select"
                    value={products}
                    onChange={handleProductChange}
                >
                    <MenuItem value={10}>Calçados</MenuItem>
                    <MenuItem value={20}>Roupas</MenuItem>
                    <MenuItem value={30}>Cosmesticos</MenuItem>
                </Select>
            </FormControl>
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

            <div className={inputStyles.button}>
                <Button type="submit" variant="contained" color="primary">Adicionar</Button>
            </div>
        </form>

    )
}