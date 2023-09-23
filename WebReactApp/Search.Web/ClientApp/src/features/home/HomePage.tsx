import { useEffect, useState } from 'react'
import { Link } from "react-router-dom";
import { Button, Container, Header, Image, Segment } from "semantic-ui-react";
import { toast } from 'react-toastify';
import { useStore } from '../../stores/Store';
import LoadingComponent from '../../layout/LoadingComponent';
import { IndexData } from '../../models/indexData';
import { observer } from "mobx-react-lite";

export default observer(function HomePage() {
  const { indexStore } = useStore();

  useEffect(() => {
    if (indexStore.initialized == false) {
      indexStore.loadIndexes();
      toast.info("Load that data!")
    }
  }, [])

  function renderIndexTable() {
    return (
      <>
        <table className='table table-striped' aria-labelledby="tabelLabel">
          <thead>
            <tr>
              <th>Index Name</th>
              <th>Created</th>
              <th>Number of documents</th>
              <th>Page</th>
            </tr>
          </thead>
          <tbody>
            {indexStore.indexList.map((oneIndex: IndexData) =>
              <tr key={oneIndex.name}>
                <td>{oneIndex.name}</td>
                <td>{oneIndex.created ? "Yes" : "No"}</td>
                <td>{oneIndex.numberOfDocuments}</td>
                <td><Button as={Link} to={`/${oneIndex.routeName}`} >Go to {oneIndex.name}</Button></td>
              </tr>
            )}
          </tbody>
        </table>
      </>
    );
  }

  if (indexStore.loading) return <LoadingComponent />;

  return (
    <Segment textAlign="center" vertical className="masthead">
      <Container>
        <Header as='h1' >
          React Example
        </Header>
        <>
          <Header as='h2' content='Welcome to React Cognitive Search Example' />
          <Button as={Link} to='/Hotels' size='huge' >
            Go to Hotels!
          </Button>
        </>

        {renderIndexTable()}
      </Container>
    </Segment>
  );
});