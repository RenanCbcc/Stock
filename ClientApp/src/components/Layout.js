import React, { Component } from 'react';
import { Container } from 'reactstrap';
import { NavMenu } from './NavMenu';
import { Route } from 'react-router';
import { Home } from '../components/Home';
import Client from '../components/client/Client';
import Product from '../components/product/Product'
import Category from '../components/category/Category'
import Supplier from '../components/supplier/Supplier'

export class Layout extends Component {
    static displayName = Layout.name;

    render() {
        return (
            <div>
                <NavMenu />
                <Container >
                    <Route exact path='/' component={Home} />
                    <Route path='/supplier' component={Supplier} />
                    <Route path='/category' component={Category} />
                    <Route path='/product' component={Product} />
                    <Route path='/client' component={Client} />
                </Container>
            </div>
        );
    }
}
