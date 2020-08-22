import React, { Component } from 'react';
import MaterialTable from 'material-table';

export class Product extends Component {

    static displayName = Product.name;

    constructor(props) {
        super(props);
        this.state = { products: [], loading: true };
    }

    componentDidMount() {
        this.populateProductsData();
    }

    renderProductsTable(products) {
        return (
            <MaterialTable
                title="Produtos"
                data={this.state.products}
                columns={[
                    { title: 'Descrição', field: 'description' },
                    { title: 'Código', field: 'code' },
                    { title: 'Preço de compra', field: 'purchasePrice', type: 'numeric' },
                    { title: 'Preço de venda', field: 'salePrice', type: 'numeric' },
                    { title: 'Quantidade', field: 'quantity', type: 'numeric' },
                    { title: 'Desconto', field: 'discount', type: 'numeric' },
                ]}

                editable={{
                    onRowAdd: newData =>
                        new Promise((resolve, reject) => {

                        }),
                }}
            />
        );
    }

    render() {
        let contents = this.state.loading
            ? <p><em>Loading...</em></p>
            : this.renderProductsTable();

        return (
            <div>
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
        await fetch('api/Product')
            .then(res => this.isOK(res))
            .then(response => response.json())
            .then(data => {
                this.setState({ products: data, loading: false });
            })
            .catch(err => console.log(err));
    }

    async saveAndAddProduct() {
        return fetch('api/Product',
            {
                method: 'Post',
                headers: { 'Content-type': 'application/json' },
                body: newData
            })
            .then(res => this.isOK(res))
            .then(response => response.json())
            .then(data => {
                let newProductsArray = [...this.state.products, data];
                let newState = { products: newProductsArray, loading: false };
                this.setState(newState)
            })
            .catch(err => console.log(err));


    }
}
