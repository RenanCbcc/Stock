import React, { Component } from 'react';

export class List extends Component {

    static displayName = List.name;

    constructor(props) {
        super(props);
        this.state = { products: [], loading: true };
    }

    componentDidMount() {
        this.populateProductsData();
    }

    static renderProductsTable(products) {
        return (
            <table className='table table-striped' aria-labelledby="tabelLabel">
                <thead>
                    <tr>
                        <th>Descrição</th>
                        <th>Preço</th>
                        <th>Código(F)</th>
                        <th>Quantidade</th>
                    </tr>
                </thead>
                <tbody>
                    {products.map((product, index) =>
                        <tr key={index}>
                            <td>{product.description}</td>
                            <td>{product.price}</td>
                            <td>{product.code}</td>
                            <td>{product.quantity}</td>
                        </tr>
                    )}
                </tbody>
            </table>
        );
    }

    render() {
        let contents = this.state.loading
            ? <p><em>Loading...</em></p>
            : List.renderProductsTable(this.state.products);

        return (
            <div>
                <h1 id="tabelLabel" >Produtos</h1>
                <p>Buscando dados no servidor. Aguarde.</p>
                {contents}
            </div>
        );
    }

    isOK(res) {
        if (res.ok) {
            return res;
        } else {
            throw new Error(res.statusText);
        }
    }

    async populateProductsData() {
        await fetch('product')
            .then(res => this.isOK(res))
            .then(response => response.json())
            .then(data => {
                this.setState({ products: data, loading: false });
            })
            .catch(err => console.log(err));
    }
}
