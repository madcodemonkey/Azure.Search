import { useEffect, useState } from 'react'
import { Link } from "react-router-dom";
import { Button, Container, Header, Image, Segment } from "semantic-ui-react";
import { toast } from 'react-toastify';
import { useStore } from '../../stores/Store';
import LoadingComponent from '../../layout/LoadingComponent';
import { IndexData } from '../../models/indexData';
import { observer } from "mobx-react-lite";
import NavBar from '../../layout/NavBar';

export default observer(function HotelsPage() {
  const { indexStore } = useStore();

  useEffect(() => {
    if (indexStore.initialized == false) {
      indexStore.loadIndexes();
      toast.info("Load that data!")
    }
  }, [])

  if (indexStore.loading) return <LoadingComponent />;

  return (
    <>
    <NavBar />
        <Segment textAlign="center" vertical className="masthead">
      <Container>
        <Header as='h1' >
          React Example
        </Header>
        <>
          <Header as='h2' content='Hotels!!' />
          <Button as={Link} to='/' size='huge' >
            Go to Home!
          </Button>
        </>
    </Container>
    </Segment>

    </>
  );
});