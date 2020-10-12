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
    const [currentCategory, setcurrentCategory] = useState('');
    const [currentProduct, setcurrentProduct] = React.useState('');
    const [price, setPrice] = useState('');
    const [quantity, setQuantity] = useState('');
    const [quantityerror, setQuantityErrors] = useState({ quantity: { valid: true, text: "" } });

    const handleCategoryChange = (event) => {
        let categoryId = event.target.value;
        setcurrentCategory(categoryId)
        fetch(`api/Product/ByCategory?id=${categoryId}`)
            .then(res => isOk(res))
            .then(response => response.json())
            .then(data => { setProducts(data) })
            .catch(err => console.log(err));
    };

    const handleProductChange = (event) => {
        let productId = event.target.value;
        setcurrentProduct(productId)
        let p = products.find(p => p.id === productId);
        setPrice(p.salePrice);
    };

    const selectStyles = useSelectStyles();
    const inputStyles = useInputStyles();

    const onSubmit = (event) => {
        event.preventDefault();
        let p = products.find(p => p.id === currentProduct)
        let code = p.code;
        let description = p.description;
        setcurrentProduct('')
        setPrice('');
        setQuantity('');
        props.onAdd({ code, description, price, quantity, subtotal: price * quantity });
    }

    const isOk = (response) => {
        if (response !== null && response.ok) {
            return response;
        } else {
            throw new Error(response.statusText);
        }
    }

    useEffect(() => {
        console.log("useEffect");
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
                    value={currentCategory}
                    onChange={handleCategoryChange}
                >
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
                    value={currentProduct}
                    onChange={handleProductChange}
                >
                    {products.map((product) =>
                        <MenuItem value={product.id}>{product.description}</MenuItem>
                    )}
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