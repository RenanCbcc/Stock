﻿import React from "react";
import { Switch, Route } from "react-router-dom";

import Client from '../components/client/Client';
import Product from '../components/product/Product'
import Category from '../components/category/Category'
import Supplier from '../components/supplier/Supplier'
import Report from '../components/report/Report'
import Sale from '../components/sale/Sale'
import Order from '../components/order/Order'

export default function Routes() {
    return (
        <Switch>
            <Route exact path='/' component={Report} />
            <Route path='/supplier' component={Supplier} />
            <Route path='/category' component={Category} />
            <Route path='/product' component={Product} />
            <Route path='/client' component={Client} />
            <Route path='/order' component={Order} />
            <Route path='/sale/:clientId' component={Sale} />
        </Switch>
    )

}