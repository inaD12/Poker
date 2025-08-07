import lobbyService from "../../../lobby/services/lobby.services";
import LobbiesPage from "./components/LobbiesPage";

const LobbiesDefaultPage = async () => {

  var reponse = await lobbyService.getAll(1,6);

  return <LobbiesPage page={1} response={reponse} />;
};

export default LobbiesDefaultPage;