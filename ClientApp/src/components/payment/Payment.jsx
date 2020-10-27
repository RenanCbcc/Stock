import React, { useState, useEffect } from 'react';
import { makeStyles } from '@material-ui/core/styles';
import TextField from '@material-ui/core/TextField';
import Button from '@material-ui/core/Button';
import Alert from '@material-ui/lab/Alert';

const baseURL = "api/Payment";

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


export default function Payment(props) {

    const [disabled, setDisabled] = useState(true);
    const [amount, setAmount] = useState('');
    const [value, setValue] = useState('');
    const [valueerror, setValueError] = useState({ value: { valid: true, text: "" } });
    const [errorMessages, setErrorMessages] = useState('');
    const [successMessages, setSuccessMessages] = useState('');
    const [iserror, setIserror] = useState(false);
    const [ismessage, setIsmessage] = useState(false);

    const clientId = props.match.params.clientId;

    const inputStyles = useInputStyles();

    const onSubmit = (event) => {
        event.preventDefault();
        let payment = {
            clientId: Number(clientId),
            value: Number(value)
        }

        fetch(baseURL, {
            method: 'Post',
            headers: { 'Content-type': 'application/json' },
            body: JSON.stringify(payment)
        })
            .then(res => isOk(res))
            .then(response => response.json())
            .then(payment => {
                console.log(payment);
                setValue('');
                setSuccessMessages('Pagamento enviado com sucesso!')
                setIsmessage(true);
                setErrorMessages('');
                setIserror(false)
            })
            .catch(error => {
                setErrorMessages([`Não foi possível enviar os dados ao servidor. ${error}`])
                setIserror(true);
                setSuccessMessages('');
                setIsmessage(false);
            })

    }

    const isOk = (response) => {
        if (response !== null && response.ok) {
            return response;
        } else {
            throw new Error(response.statusText);
        }
    }

    useEffect(() => {
        fetch(`/api/Order/ClientAmount/?clientId=${clientId}`)
            .then(res => isOk(res))
            .then(response => response.json())
            .then(data => { setAmount(data) })
            .catch(err => console.log(err));
    }, []);

    useEffect(() => {
        if (value <= 0 || value > amount) {
            setDisabled(true);
        } else {
            setDisabled(false);
        }
    });
    return (
        <>
            <div>
                {iserror &&
                    <Alert severity="error">{errorMessages}</Alert>
                }
                {ismessage &&
                    <Alert severity="success">{successMessages}</Alert>
                }

            </div>
            <form className={inputStyles.text} autoComplete="off" onSubmit={onSubmit}>
                <TextField
                    disabled id="amount"
                    value={amount}
                    label="Total"
                    variant="outlined"
                />
                <TextField
                    required type="number"
                    id="valut"
                    label="Valor"
                    variant="outlined"
                    value={value}
                    error={!valueerror.value.valid}
                    helperText={valueerror.value.text}
                    onChange={(event) => {
                        let v = event.target.value;
                        if (v <= 0 || v > amount) {
                            setValueError(
                                {
                                    value:
                                    {
                                        valid: false,
                                        text: `O valor precisa ser maior que 0 e menor que ${amount}`
                                    }
                                });
                        } else {
                            setValueError({ value: { valid: true, text: "" } });
                        }
                        setValue(v);
                    }}
                />

                <div className={inputStyles.button}>
                    <Button disabled={disabled} type="submit" variant="contained" color="primary">Debitar</Button>
                </div>
            </form>
        </>);

}