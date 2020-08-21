import React, { Component } from 'react';
import MaterialTable from 'material-table';

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
            <></>
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
        await fetch('Product')
            .then(res => this.isOK(res))
            .then(response => response.json())
            .then(data => {
                this.setState({ products: data, loading: false });
            })
            .catch(err => console.log(err));
    }
}
